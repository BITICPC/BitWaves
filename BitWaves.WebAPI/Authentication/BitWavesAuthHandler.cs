using System;
using System.Net;
using System.Net.Http.Headers;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using BitWaves.WebAPI.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BitWaves.WebAPI.Authentication
{
    /// <summary>
    /// 为 BitWaves 提供自定义权限验证中间件。
    /// </summary>
    public sealed class BitWavesAuthHandler : AuthenticationHandler<BitWavesAuthOptions>
    {
        private readonly IJwtService _jwtService;

        /// <summary>
        /// 初始化 <see cref="BitWavesAuthHandler"/> 的新实例。
        /// </summary>
        public BitWavesAuthHandler(IOptionsMonitor<BitWavesAuthOptions> options,
                                   ILoggerFactory logger,
                                   UrlEncoder encoder,
                                   ISystemClock clock,
                                   IJwtService jwtService)
            : base(options, logger, encoder, clock)
        {
            _jwtService = jwtService;
        }

        /// <inheritdoc />
        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!AuthenticationHeaderValue.TryParse(Request.Headers["Authorization"], out var headerValue))
            {
                return AuthenticateResult.NoResult();
            }

            if (!headerValue.Scheme.Equals("Jwt", StringComparison.OrdinalIgnoreCase))
            {
                return AuthenticateResult.NoResult();
            }

            var jwt = headerValue.Parameter;
            BitWavesAuthenticationToken token;
            try
            {
                token = _jwtService.Decode<BitWavesAuthenticationToken>(jwt);
            }
            catch (Exception ex)
            {
                return AuthenticateResult.Fail(ex);
            }

            var ticket = token.GetAuthenticationTicket();
            return AuthenticateResult.Success(ticket);
        }

        /// <inheritdoc />
        protected override Task HandleChallengeAsync(AuthenticationProperties properties)
        {
            // In BitWavesAuthHandler we need to return 403 instead of 401 since all authentication information
            // is available.
            if (Context.Response.HasStarted)
            {
                return Task.CompletedTask;
            }

            Context.Response.StatusCode = (int) HttpStatusCode.Forbidden;
            return Task.CompletedTask;
        }
    }
}
