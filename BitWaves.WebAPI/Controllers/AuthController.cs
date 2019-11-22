using System.Threading.Tasks;
using BitWaves.Data;
using BitWaves.Data.Entities;
using BitWaves.WebAPI.Authentication;
using BitWaves.WebAPI.Models;
using BitWaves.WebAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace BitWaves.WebAPI.Controllers
{
    [Route("auth")]
    [ApiController]
    public sealed class AuthController : ControllerBase
    {
        private readonly Repository _repo;
        private readonly IJwtService _jwt;
        private readonly ILogger<AuthController> _logger;

        public AuthController(Repository repo, IJwtService jwt, ILogger<AuthController> logger)
        {
            _repo = repo;
            _jwt = jwt;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = await _repo.Users.Find(Builders<User>.Filter.Eq(u => u.Username, model.Username))
                                  .FirstOrDefaultAsync();
            if (user == null)
            {
                // 用户不存在
                return NotFound();
            }

            if (!user.Challenge(model.Password))
            {
                // 密码错误
                return UnprocessableEntity();
            }

            var authToken = new BitWavesAuthenticationToken(user);
            var authTokenJwt = _jwt.Encode(authToken);

            var result = new LoginResult(user, authTokenJwt);
            return new ObjectResult(result);
        }
    }
}
