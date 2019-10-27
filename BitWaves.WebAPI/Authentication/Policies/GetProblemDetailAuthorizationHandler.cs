using System.Threading.Tasks;
using BitWaves.Data.Entities;
using Microsoft.AspNetCore.Authorization;

namespace BitWaves.WebAPI.Authentication.Policies
{
    /// <summary>
    /// 为用户获取题目详细信息提供权限验证逻辑。
    /// </summary>
    public sealed class GetProblemDetailAuthorizationHandler :
        AuthorizationHandler<GetProblemDetailRequirement, Problem>
    {
        /// <inheritdoc />
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            GetProblemDetailRequirement requirement,
            Problem resource)
        {
            if (resource.ArchiveId != null)
            {
                // Public problem is accessible by everyone.
                context.Succeed(requirement);
                return Task.CompletedTask;
            }

            // TODO: Update the authorization logic of accessing problem details below.
            // Otherwise the user need to be an administrator to access the detail of the problem.
            if (context.User.IsInRole(BitWavesAuthRoles.Admin))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
