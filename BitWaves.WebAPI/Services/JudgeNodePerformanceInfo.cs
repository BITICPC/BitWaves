using Newtonsoft.Json;

namespace BitWaves.WebAPI.Services
{
    /// <summary>
    /// 提供评测节点的性能信息。
    /// </summary>
    public sealed class JudgeNodePerformanceInfo
    {
        /// <summary>
        /// 获取或设置评测节点上的CPU占用率。
        /// </summary>
        [JsonProperty("cpuUsage")]
        public double CpuUsage { get; set; }

        /// <summary>
        /// 获取或设置评测节点上安装的CPU核心数量。
        /// </summary>
        [JsonProperty("cores")]
        public int Cores { get; set; }

        /// <summary>
        /// 获取或设置评测节点上安装的总物理内存量，单位为 MB。
        /// </summary>
        [JsonProperty("totalPhysicalMemory")]
        public long TotalPhysicalMemory { get; set; }

        /// <summary>
        /// 获取或设置评测节点上当前可用的物理内存量，单位为 MB。
        /// </summary>
        [JsonProperty("freePhysicalMemory")]
        public long FreePhysicalMemory { get; set; }

        /// <summary>
        /// 获取或设置评测节点上的页交换文件大小，单位为 MB。
        /// </summary>
        [JsonProperty("swapFileSize")]
        public long SwapFileSize { get; set; }

        /// <summary>
        /// 获取或设置评测节点上已缓存的页交换文件大小，单位为 MB。
        /// </summary>
        [JsonProperty("cachedSwapSpace")]
        public long CachedSwapSpace { get; set; }
    }
}
