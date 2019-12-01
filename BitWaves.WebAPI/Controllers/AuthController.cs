using System.Threading.Tasks;
using BitWaves.Data.Repositories;
using BitWaves.WebAPI.Authentication;
using BitWaves.WebAPI.Models;
using BitWaves.WebAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

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

        // POST: /auth
        [HttpPost]
        public async Task<ActionResult<LoginResult>> Login([FromBody] LoginModel model)
        {
            var entity = await _repo.Users.FindOneAsync(model.Username);
            if (entity == null)
            {
                return NotFound();
            }

            if (!entity.Challenge(model.Password))
            {
                return UnprocessableEntity();
            }

            var authToken = new BitWavesAuthenticationToken(entity);
            var authTokenJwt = _jwt.Encode(authToken);

            return new LoginResult(entity, authTokenJwt);
        }
    }
}
