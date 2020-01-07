using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace BitWaves.WebAPI.Services
{
    /// <summary>
    /// 提供获取评测集群信息的服务抽象。
    /// </summary>
    public interface IJudgeService
    {
        /// <summary>
        /// 获取所有在线的评测节点信息。
        /// </summary>
        /// <returns>所有在线的评测节点信息。</returns>
        Task<List<JudgeNodeInfo>> GetJudgeNodesAsync();
    }

    /// <summary>
    /// 为 <see cref="IJudgeService"/> 提供扩展方法。
    /// </summary>
    public static class JudgeServiceExtensions
    {
        /// <summary>
        /// 将 <see cref="IJudgeService"/> 的默认实现添加到依赖服务集中。
        /// </summary>
        /// <param name="services"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static IServiceCollection AddDefaultJudgeService(
            this IServiceCollection services,
            Action<JudgeServiceOptions> options)
        {
            Contract.NotNull(services, nameof(services));
            Contract.NotNull(options, nameof(options));

            var opt = new JudgeServiceOptions();
            options(opt);

            services.AddHttpClient<IJudgeService, DefaultJudgeService>(
                config => config.BaseAddress = new Uri(opt.JudgeBoardAddress));
            return services;
        }
    }
}
