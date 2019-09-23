using BitWaves.WebAPI.Models.Internals;

namespace BitWaves.WebAPI.Features
{
    /// <summary>
    /// 为 BitWaves 身份验证提供 Feature。
    /// </summary>
    internal sealed class BitWavesAuthenticationFeature
    {
        /// <summary>
        /// 初始化 <see cref="BitWavesAuthenticationFeature"/> 类的新实例。
        /// </summary>
        /// <param name="token">当前 HTTP 上下文中所包含的用户身份验证标识。</param>
        public BitWavesAuthenticationFeature(AuthenticationToken token)
        {
            AuthenticationToken = token;
        }

        /// <summary>
        /// 获取当前 HTTP 请求中所包含的身份验证标识。若当前的 HTTP 请求不包含身份验证标识，返回 null。
        /// </summary>
        public AuthenticationToken AuthenticationToken { get; }
    }
}
