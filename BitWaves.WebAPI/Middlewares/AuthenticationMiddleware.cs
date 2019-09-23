using System.Net;
using System.Threading.Tasks;
using BitWaves.WebAPI.Features;
using BitWaves.WebAPI.Models.Internals;
using BitWaves.WebAPI.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace BitWaves.WebAPI.Middlewares
{
    /// <summary>
    /// 提供 BitWaves 应用程序自定义的身份验证中间件。
    /// </summary>
    // ReSharper disable once ClassNeverInstantiated.Global
    internal sealed class AuthenticationMiddleware
    {
        private readonly RequestDelegate _next;

        /// <summary>
        /// 初始化 <see cref="AuthenticationMiddleware"/> 的新实例。
        /// </summary>
        /// <param name="next">中间件管道上的下一个中间件的委托代理。</param>
        public AuthenticationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        /// <summary>
        /// 调用中间件逻辑。
        /// </summary>
        /// <param name="context">HTTP 上下文对象。</param>
        /// <param name="jwtService">JWT 组件。</param>
        /// <returns>异步操作句柄。</returns>
        public async Task InvokeAsync(HttpContext context, IJwtService jwtService)
        {
            if (!context.Request.Headers.TryGetValue("Jwt", out var jwtValues))
            {
                await _next(context);
                return;
            }

            if (jwtValues.Count > 1)
            {
                context.Response.StatusCode = (int) HttpStatusCode.BadRequest;
                return;
            }

            AuthenticationToken token;
            try
            {
                token = jwtService.Decode<AuthenticationToken>(jwtValues[0]);
            }
            catch
            {
                context.Response.StatusCode = (int) HttpStatusCode.BadRequest;
                return;
            }

            context.Features.Set(new BitWavesAuthenticationFeature(token));

            await _next(context);
        }
    }

    /// <summary>
    /// 为 <see cref="AuthenticationMiddleware"/> 提供扩展逻辑。
    /// </summary>
    internal static class AuthenticationMiddlewareExtensions
    {
        /// <summary>
        /// 将 <see cref="AuthenticationMiddleware"/> 注册到应用程序中间件管道中。
        /// </summary>
        /// <param name="builder">用于建造中间件管道的应用程序建造器。</param>
        /// <returns>注册后的 <see cref="IApplicationBuilder"/> 对象。</returns>
        public static IApplicationBuilder UseBitWavesAuthentication(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AuthenticationMiddleware>();
        }
    }
}
