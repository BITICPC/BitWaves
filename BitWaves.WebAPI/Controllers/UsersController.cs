using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using BitWaves.Data;
using BitWaves.Data.Entities;
using BitWaves.WebAPI.Authentication;
using BitWaves.WebAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
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

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserModel model)
        {
            var user = model.ToUserEntity();

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

        [HttpGet("{username}")]
        public async Task<IActionResult> GetUserInfo(string username, bool detailed = false)
        {
            var entity = await _repo.Users.Find(Builders<User>.Filter.Eq(u => u.Username, username))
                                    .FirstOrDefaultAsync();
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
                    return Forbid();
                }
            }

            var model = _mapper.Map<User, UserInfo>(entity);
            return new ObjectResult(model);
        }

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

            var updates = model.GetUpdateDefinition();
            await _repo.Users.UpdateOneAsync(Builders<User>.Filter.Eq(u => u.Username, username),
                                             Builders<User>.Update.Combine(updates));
            return Ok();
        }

        private Expression<Func<User, object>> GetRanklistKeySelector(RanklistKey key)
        {
            // TODO: Refactor GetRanklistKeySelector to an extension method of RanklistKey enum.
            switch (key)
            {
                case RanklistKey.TotalAccepted:
                    return u => u.TotalAcceptedSubmissions;
                case RanklistKey.TotalSubmissions:
                    return u => u.TotalSubmissions;
                case RanklistKey.TotalProblemsAccepted:
                    return u => u.TotalProblemsAccepted;
                case RanklistKey.TotalProblemsAttempted:
                    return u => u.TotalProblemsAttempted;
                default:
                    throw new Exception("Unreachable code.");
            }
        }

        [HttpGet("ranklist")]
        public async Task<IActionResult> GetRanklist(
            [FromQuery][BindRequired] RanklistKey by,
            [FromQuery][Range(1, int.MaxValue)] int limit = 20)
        {
            var entities = await _repo.Users.Find(Builders<User>.Filter.Empty)
                                      .SortByDescending(GetRanklistKeySelector(by))
                                      .Limit(limit)
                                      .ToListAsync();

            var models = entities.Select(e => _mapper.Map<User, UserInfo>(e));
            return new ObjectResult(models);
        }
    }
}
