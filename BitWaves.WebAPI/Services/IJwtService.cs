using Microsoft.Extensions.DependencyInjection;

namespace BitWaves.WebAPI.Services
{
    /// <summary>
    /// 为 JWT 编码、解码服务提供抽象。
    /// </summary>
    public interface IJwtService
    {
        /// <summary>
        /// 将给定的值编码为 JWT 形式。
        /// </summary>
        /// <param name="value">要编码的值。</param>
        /// <typeparam name="T">要编码的值的类型。</typeparam>
        /// <returns>编码后的 JWT。</returns>
        string Encode<T>(T value);

        /// <summary>
        /// 从给定的 JWT 片段中解码给定类型的实例。
        /// </summary>
        /// <param name="jwt">JWT 片段。</param>
        /// <typeparam name="T">要解码的值的类型。</typeparam>
        /// <returns>解码后的值。</returns>
        T Decode<T>(string jwt);
    }

    namespace DependencyInjection
    {
        /// <summary>
        /// 为 <see cref="IJwtService"/> 提供依赖注入逻辑。
        /// </summary>
        public static class JwtServiceExtensions
        {
            /// <summary>
            /// 向依赖服务集中添加不带加密和签名的 JWT 服务。
            /// </summary>
            /// <param name="services">依赖服务集。</param>
            /// <returns>添加 JWT 服务后的依赖服务集。</returns>
            public static IServiceCollection AddPlainJoseJwt(this IServiceCollection services)
            {
                Contract.NotNull(services, nameof(services));

                return services.AddSingleton<IJwtService, PlainJoseJwtService>();
            }
        }
    }
}
