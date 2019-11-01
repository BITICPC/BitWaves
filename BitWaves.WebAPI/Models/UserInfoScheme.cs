namespace BitWaves.WebAPI.Models
{
    /// <summary>
    /// 表示用户信息的使用场景。
    /// </summary>
    public enum UserInfoScheme
    {
        /// <summary>
        /// 仅使用用户的公开信息。
        /// </summary>
        PublicInfo,

        /// <summary>
        /// 使用用户的全部信息。
        /// </summary>
        Full
    }
}
