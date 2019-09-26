using System;
using System.Net;
using BitWaves.WebAPI.Extensions;
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
            var authToken = context.HttpContext.GetAuthenticationToken();
            if (authToken == null)
            {
                // 请求不携带有效的身份验证标识，返回 403 Forbidden
                // 注意：不要使用 ForbidResult，因为我们没有采用 ASP.Net Core MVC 内置的身份验证机制
                context.Result = new StatusCodeResult((int) HttpStatusCode.Forbidden);
                return;
            }

            if (RequireAdmin && !authToken.IsAdmin)
            {
                context.Result = new ForbidResult();
            }
        }
    }
}
