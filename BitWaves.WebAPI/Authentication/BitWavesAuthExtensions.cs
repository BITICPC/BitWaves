using System;
using BitWaves.WebAPI.Utils;
using Microsoft.AspNetCore.Authentication;

namespace BitWaves.WebAPI.Authentication
{
    /// <summary>
    /// 为 BitWaves 身份验证中间件提供依赖注入逻辑。
    /// </summary>
    internal static class BitWavesAuthExtensions
    {
        /// <summary>
        /// 添加 BitWaves 身份验证中间件。
        /// </summary>
        /// <param name="builder">身份验证场景构建器。</param>
        /// <param name="options">选项设置委托。</param>
        /// <returns>身份验证场景构建器。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="builder"/> 为 null。</exception>
        public static AuthenticationBuilder AddBitWavesScheme(this AuthenticationBuilder builder,
                                                              Action<BitWavesAuthOptions> options = null)
        {
            Contract.NotNull(builder, nameof(builder));

            if (options == null)
            {
                options = opt => { };
            }

            return builder.AddScheme<BitWavesAuthOptions, BitWavesAuthHandler>(
                BitWavesAuthDefaults.SchemeName, options);
        }
    }
}
