using Microsoft.AspNetCore.Authorization;

namespace BitWaves.WebAPI.Authentication.Policies
{
    /// <summary>
    /// 一个标记类，表示用户获取题目详细信息的权限验证需求。
    /// </summary>
    public sealed class GetProblemDetailRequirement : IAuthorizationRequirement
    {
    }
}
