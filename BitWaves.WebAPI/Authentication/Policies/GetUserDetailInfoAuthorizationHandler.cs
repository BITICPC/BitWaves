using System.Threading.Tasks;
using BitWaves.Data.Entities;
using Microsoft.AspNetCore.Authorization;

namespace BitWaves.WebAPI.Authentication.Policies
{
    /// <summary>
    /// 为访问用户详细信息场景提供权限验证处理逻辑。
    /// </summary>
    public sealed class GetUserDetailInfoAuthorizationHandler :
        AuthorizationHandler<GetUserDetailInfoRequirement, User>
    {
        /// <inheritdoc />
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            GetUserDetailInfoRequirement requirement,
            User resource)
        {
            if (context.User.IsInRole(BitWavesAuthRoles.Admin))
            {
                // Admin can access all information.
                context.Succeed(requirement);
                return Task.CompletedTask;
            }

            if (context.User.Identity.Name == resource.Username)
            {
                // The user himself / herself can always access his / her own info.
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
