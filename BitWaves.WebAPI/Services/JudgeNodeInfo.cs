using System;
using Newtonsoft.Json;

namespace BitWaves.WebAPI.Services
{
    /// <summary>
    /// 提供评测节点的相关信息。
    /// </summary>
    public sealed class JudgeNodeInfo
    {
        /// <summary>
        /// 获取或设置评测节点的地址。
        /// </summary>
        [JsonProperty("address")]
        public string Address { get; set; }

        /// <summary>
        /// 获取或设置评测节点的上一个心跳包的时间戳。
        /// </summary>
        [JsonProperty("lastHeartBeat")]
        public DateTime LastHeartBeat { get; set; }

        /// <summary>
        /// 获取或设置评测节点发送的上一个网络数据包的时间戳。
        /// </summary>
        [JsonProperty("lastSeen")]
        public DateTime LastSeen { get; set; }

        /// <summary>
        /// 获取或设置当前交付给评测节点执行评测的提交数量。
        /// </summary>
        [JsonProperty("queuedSubmissions")]
        public int QueuedSubmissions { get; set; }

        /// <summary>
        /// 获取或设置一个值，该值指示当前评测节点是否被临时隔离。
        /// </summary>
        [JsonProperty("isBlocked")]
        public bool IsBlocked { get; set; }

        /// <summary>
        /// 获取或设置评测节点的性能信息。
        /// </summary>
        [JsonProperty("performance")]
        public JudgeNodePerformanceInfo Performance { get; set; }
    }
}
