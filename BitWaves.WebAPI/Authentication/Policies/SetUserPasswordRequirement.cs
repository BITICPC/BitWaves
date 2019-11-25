using Microsoft.AspNetCore.Authorization;

namespace BitWaves.WebAPI.Authentication.Policies
{
    /// <summary>
    /// 为设置用户密码的权限检查策略提供 <see cref="IAuthorizationRequirement"/> 标签对象。
    /// </summary>
    public sealed class SetUserPasswordRequirement : IAuthorizationRequirement
    {
        /// <summary>
        /// 获取或设置用户提供的旧密码。
        /// </summary>
        public string OldPassword { get; set; }
    }
}
