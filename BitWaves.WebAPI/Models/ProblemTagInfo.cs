using Newtonsoft.Json;

namespace BitWaves.WebAPI.Models
{
    /// <summary>
    /// 为题目标签提供信息。
    /// </summary>
    public sealed class ProblemTagInfo
    {
        /// <summary>
        /// 获取题目标签的名称。
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; private set; }

        /// <summary>
        /// 获取包含此标签的题目数量。
        /// </summary>
        [JsonProperty("problems")]
        public int Problems { get; private set; }
    }
}
