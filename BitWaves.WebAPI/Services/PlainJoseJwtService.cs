using Jose;

namespace BitWaves.WebAPI.Services
{
    /// <summary>
    /// 提供基于 jose-jwt 的，不带签名和加密的 JWT 编码、解码服务。
    /// </summary>
    internal sealed class PlainJoseJwtService : IJwtService
    {
        /// <inheritdoc cref="IJwtService.Encode{T}"/>
        public string Encode<T>(T value)
        {
            return JWT.Encode(value, null, JwsAlgorithm.none);
        }

        /// <inheritdoc cref="IJwtService.Decode{T}"/>
        public T Decode<T>(string jwt)
        {
            return JWT.Decode<T>(jwt, null, JwsAlgorithm.none);
        }
    }
}
