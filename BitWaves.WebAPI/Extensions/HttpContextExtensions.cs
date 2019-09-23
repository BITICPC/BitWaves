using BitWaves.WebAPI.Features;
using BitWaves.WebAPI.Models.Internals;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;

namespace BitWaves.WebAPI.Extensions
{
    /// <summary>
    /// 为 <see cref="IFeatureCollection"/> 提供扩展方法。
    /// </summary>
    internal static class HttpContextExtensions
    {
        /// <summary>
        /// 获取给定 HTTP 上下文中所包含的用户身份验证标识。
        /// </summary>
        /// <param name="context">HTTP 上下文。</param>
        /// <returns>给定的 HTTP 上下文中所包含的用户身份验证标识。</returns>
        public static AuthenticationToken GetAuthenticationToken(this HttpContext context)
        {
            return context.Features.Get<BitWavesAuthenticationFeature>().AuthenticationToken;
        }
    }
}
