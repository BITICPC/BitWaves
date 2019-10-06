using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using BitWaves.Data;
using BitWaves.Data.Entities;
using BitWaves.WebAPI.Extensions;
using BitWaves.WebAPI.Filters;
using BitWaves.WebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using MongoDB.Driver;

namespace BitWaves.WebAPI.Controllers
{
    [Route("users")]
    [ApiController]
    public sealed class UsersController : ControllerBase
    {
        private readonly Repository _repo;

        public UsersController(Repository repo)
        {
            _repo = repo;
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
                    return this.ErrorMessage(1, "用户名已经存在");
                }

                throw;
            }

            return Ok();
        }

        [HttpGet("{username}")]
        public async Task<IActionResult> GetUserInfo(string username, bool detailed = false)
        {
            if (detailed)
            {
                var authToken = HttpContext.GetAuthenticationToken();
                if (authToken == null || (authToken.Username != username && !authToken.IsAdmin))
                {
                    return Forbid();
                }
            }

            var entity = await _repo.Users.Find(Builders<User>.Filter.Eq(u => u.Username, username))
                                    .FirstOrDefaultAsync();
            if (entity == null)
            {
                return NotFound();
            }

            return new ObjectResult(new UserInfo(entity, detailed));
        }

        [HttpPut("{username}")]
        [RequireAuthentication]
        public async Task<IActionResult> SetUserInfo(
            string username,
            [FromBody] UpdateUserModel model)
        {
            var authToken = HttpContext.GetAuthenticationToken();
            if (authToken.Username != username && !authToken.IsAdmin)
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

            return new ObjectResult(entities.Select(e => new UserRanklistInfo(e)));
        }
    }
}
