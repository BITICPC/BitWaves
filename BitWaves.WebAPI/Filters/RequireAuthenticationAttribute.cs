using System;
using BitWaves.WebAPI.Models.Internals;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BitWaves.WebAPI.Filters
{
    /// <summary>
    /// 检查请求携带有效的身份验证标识。
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public sealed class RequireAuthenticationAttribute : Attribute, IAuthorizationFilter
    {
        /// <summary>
        /// 获取或设置一个值表示是否要求身份验证标识携带管理员权限。
        /// </summary>
        public bool RequireAdmin { get; set; }

        /// <inheritdoc />
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var authToken = context.HttpContext.Features.Get<AuthenticationToken>();
            if (authToken == null)
            {
                // 请求不携带有效的身份验证标识
                context.Result = new ForbidResult();
                return;
            }

            if (RequireAdmin && !authToken.IsAdmin)
            {
                context.Result = new ForbidResult();
            }
        }
    }
}
