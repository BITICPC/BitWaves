namespace BitWaves.WebAPI.Authentication
{
    /// <summary>
    /// 为 BitWaves 身份验证中间件提供默认常量。
    /// </summary>
    public static class BitWavesAuthDefaults
    {
        /// <summary>
        /// BitWaves 身份验证中间件的 scheme 名称。
        /// </summary>
        public const string SchemeName = "BitWaves";

        /// <summary>
        /// BitWaves 身份验证中间件签发的身份验证标识的 authentication type.
        /// </summary>
        public const string AuthenticateType = "BitWaves";

        /// <summary>
        /// BitWaves 身份验证所生成的 claim 中表示 identity 创建时间的名称。
        /// </summary>
        public const string IdentityCreationTime = "IdentityCreationTime";
    }
}
