using System.Threading.Tasks;
using BitWaves.Data.Entities;
using Microsoft.AspNetCore.Authorization;

namespace BitWaves.WebAPI.Authentication.Policies
{
    /// <summary>
    /// 为设置用户密码操作提供权限检查逻辑。
    /// </summary>
    public sealed class SetUserPasswordAuthorizationHandler
        : AuthorizationHandler<SetUserPasswordRequirement, User>
    {
        /// <inheritdoc />
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
                                                       SetUserPasswordRequirement requirement,
                                                       User resource)
        {
            if (context.User.IsInRole(BitWavesAuthRoles.Admin))
            {
                context.Succeed(requirement);
                return Task.CompletedTask;
            }

            // User is not an administrator. Check the username and challenge the user provided old password.
            if (string.CompareOrdinal(context.User.Identity.Name, resource.Username) != 0)
            {
                context.Fail();
                return Task.CompletedTask;
            }

            if (requirement.OldPassword == null)
            {
                context.Fail();
                return Task.CompletedTask;
            }

            if (!resource.Challenge(requirement.OldPassword))
            {
                context.Fail();
                return Task.CompletedTask;
            }

            context.Succeed(requirement);
            return Task.CompletedTask;
        }
    }
}
