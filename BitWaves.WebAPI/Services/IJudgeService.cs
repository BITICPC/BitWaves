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

        /// <summary>
        /// 隔离指定的评测节点。
        /// </summary>
        /// <param name="address">要隔离的评测节点的地址。</param>
        /// <param name="blocked">是否隔离评测节点。</param>
        /// <returns>是否成功地隔离或解隔离了该评测节点。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="address"/> 为 null。</exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="address"/> 为空串
        ///     或
        ///     <paramref name="address"/> 不是有效的评测节点地址。
        /// </exception>
        Task<bool> BlockJudgeNodeAsync(string address, bool blocked = true);
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
