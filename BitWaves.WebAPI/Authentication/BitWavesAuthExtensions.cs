using System;
using BitWaves.WebAPI.Authentication.Policies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace BitWaves.WebAPI.Authentication
{
    /// <summary>
    /// 为 BitWaves 身份验证中间件提供依赖注入逻辑。
    /// </summary>
    internal static class BitWavesAuthExtensions
    {
        /// <summary>
        /// 添加 BitWaves 身份验证中间件。
        /// </summary>
        /// <param name="builder">身份验证场景构建器。</param>
        /// <param name="options">选项设置委托。</param>
        /// <returns>身份验证场景构建器。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="builder"/> 为 null。</exception>
        public static AuthenticationBuilder AddBitWavesScheme(this AuthenticationBuilder builder,
                                                              Action<BitWavesAuthOptions> options = null)
        {
            Contract.NotNull(builder, nameof(builder));

            if (options == null)
            {
                options = opt => { };
            }

            return builder.AddScheme<BitWavesAuthOptions, BitWavesAuthHandler>(
                BitWavesAuthDefaults.SchemeName, options);
        }

        /// <summary>
        /// 添加 BitWaves 权限验证服务。
        /// </summary>
        /// <param name="services">服务集。</param>
        /// <returns>服务集。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="services"/> 为 null。</exception>
        public static IServiceCollection AddBitWavesAuthorization(this IServiceCollection services)
        {
            Contract.NotNull(services, nameof(services));

            services.AddAuthorization(options =>
            {
                options.AddPolicy(BitWavesAuthPolicies.AdminOnly,
                                  policyBuilder => policyBuilder.RequireRole(BitWavesAuthRoles.Admin));
                options.AddPolicy(BitWavesAuthPolicies.GetProblemDetail,
                    policyBuilder => policyBuilder.Requirements.Add(new GetProblemDetailRequirement()));
                options.AddPolicy(BitWavesAuthPolicies.GetUserDetail,
                    policyBuilder => policyBuilder.Requirements.Add(new GetUserDetailInfoRequirement()));
            });

            services.AddSingleton<IAuthorizationHandler, GetProblemDetailAuthorizationHandler>();
            services.AddSingleton<IAuthorizationHandler, GetUserDetailInfoAuthorizationHandler>();

            return services;
        }
    }
}
