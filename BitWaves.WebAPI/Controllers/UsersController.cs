using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using BitWaves.Data.DML;
using BitWaves.Data.Entities;
using BitWaves.Data.Repositories;
using BitWaves.WebAPI.Authentication;
using BitWaves.WebAPI.Authentication.Policies;
using BitWaves.WebAPI.Models;
using BitWaves.WebAPI.Utils;
using BitWaves.WebAPI.Validation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace BitWaves.WebAPI.Controllers
{
    [Route("users")]
    [ApiController]
    public sealed class UsersController : ControllerBase
    {
        private readonly IAuthorizationService _authorization;
        private readonly Repository _repo;
        private readonly IMapper _mapper;

        public UsersController(IAuthorizationService authorization, Repository repo, IMapper mapper)
        {
            _authorization = authorization;
            _repo = repo;
            _mapper = mapper;
        }

        // GET: /users
        public async Task<PaginatedListActionResult<UserListInfo>> GetUsers(
            [FromQuery] [Page] int page = 0,
            [FromQuery] [ItemsPerPage] int itemsPerPage = 1,
            [FromQuery] UserSortKey by = UserSortKey.TotalProblemsAccepted,
            [FromQuery] bool descend = true)
        {
            var findPipeline = new UserFindPipeline(UserFilterBuilder.Empty)
            {
                SortKey = by,
                SortByDescending = descend,
                Pagination = new Pagination(page, itemsPerPage)
            };

            var findResult = await _repo.Users.FindManyAsync(findPipeline);
            var models = findResult.ResultSet.Select(e => _mapper.Map<User, UserListInfo>(e));

            return (findResult.TotalCount, models);
        }

        // POST: /users
        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserModel model)
        {
            var user = _mapper.Map<CreateUserModel, User>(model);

            try
            {
                await _repo.Users.InsertOneAsync(user);
            }
            catch (MongoWriteException ex)
            {
                if (ex.WriteError.Category == ServerErrorCategory.DuplicateKey)
                {
                    // 可以认为用户名冲突引发当前的异常
                    return Conflict();
                }

                throw;
            }

            return Ok();
        }

        // GET: /users/{username}
        [HttpGet("{username}")]
        public async Task<ActionResult<UserInfo>> GetUserInfo(string username, bool detailed = false)
        {
            var entity = await _repo.Users.FindOneAsync(username);
            if (entity == null)
            {
                return NotFound();
            }

            if (detailed)
            {
                // 访问用户详细信息要求权限检查
                var authResult = await _authorization.AuthorizeAsync(User, entity, BitWavesAuthPolicies.GetUserDetail);
                if (!authResult.Succeeded)
                {
                    if (User == null)
                    {
                        return Challenge();
                    }

                    return Forbid();
                }
            }
            else
            {
                // 当 detailed 为 false 时抹去保密字段
                entity.Phone = null;
                entity.Email = null;
                entity.School = null;
                entity.StudentId = null;
            }

            var model = _mapper.Map<User, UserInfo>(entity);
            model.Rank = await _repo.Users.CountUsersWithMoreAcceptedProblemsAsync(entity.TotalProblemsAccepted) + 1;

            return model;
        }

        // PUT: /users/{username}
        [HttpPut("{username}")]
        [Authorize]
        public async Task<IActionResult> SetUserInfo(
            string username,
            [FromBody] UpdateUserModel model)
        {
            // TODO: Refactor here to use authorization policy instead of authorize manually.
            if (!HttpContext.User.HasClaim(ClaimTypes.Name, username) &&
                !HttpContext.User.HasClaim(ClaimTypes.Role, BitWavesAuthRoles.Admin))
            {
                return Forbid();
            }

            var update = _mapper.Map<UpdateUserModel, UserUpdateInfo>(model);
            await _repo.Users.UpdateOneAsync(username, update);
            return Ok();
        }

        // PUT: /users/{username}/password
        [HttpPut("{username}/password")]
        [Authorize]
        public async Task<IActionResult> SetUserPassword(
            string username,
            [FromBody] UpdateUserPasswordModel model)
        {
            var entity = await _repo.Users.FindOneAsync(username);
            if (entity == null)
            {
                return NotFound();
            }

            var requirement = new SetUserPasswordRequirement();
            if (model.OldPassword.HasValue)
            {
                requirement.OldPassword = model.OldPassword.Value;
            }

            var authResult = await _authorization.AuthorizeAsync(User, entity, requirement);
            if (!authResult.Succeeded)
            {
                return Forbid();
            }

            await _repo.Users.UpdatePasswordAsync(username, model.NewPassword);
            return Ok();
        }
    }
}
