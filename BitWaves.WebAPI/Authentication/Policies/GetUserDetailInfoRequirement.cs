using Microsoft.AspNetCore.Authorization;

namespace BitWaves.WebAPI.Authentication.Policies
{
    /// <summary>
    /// 表示获取用户信息的 <see cref="IAuthorizationRequirement"/> 标记类型。
    /// </summary>
    public sealed class GetUserDetailInfoRequirement : IAuthorizationRequirement
    {
    }
}
