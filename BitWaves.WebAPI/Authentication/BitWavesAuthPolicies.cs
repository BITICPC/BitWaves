namespace BitWaves.WebAPI.Authentication
{
    /// <summary>
    /// 提供 BitWaves 权限验证策略名称。
    /// </summary>
    public static class BitWavesAuthPolicies
    {
        /// <summary>
        /// 仅限管理员访问策略名称。
        /// </summary>
        public const string AdminOnly = "AdminOnly";

        /// <summary>
        /// 当用户尝试访问题目详细数据时的权限验证策略名称。
        /// </summary>
        public const string GetProblemDetail = "AccessProblem";

        /// <summary>
        /// 当用户尝试访问用户详细数据时的权限验证策略名称。
        /// </summary>
        public const string GetUserDetail = "AccessUserDetail";
    }
}
