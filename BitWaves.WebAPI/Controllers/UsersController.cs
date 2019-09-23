using System.Threading.Tasks;
using BitWaves.Data;
using BitWaves.WebAPI.Extensions;
using BitWaves.WebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace BitWaves.WebAPI.Controllers
{
    [Route("users")]
    [ApiController]
    public sealed class UsersController : ControllerBase
    {
        private readonly Repository _repo;
        private readonly ILogger<UsersController> _logger;

        public UsersController(Repository repo, ILogger<UsersController> logger)
        {
            _repo = repo;
            _logger = logger;
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
    }
}
