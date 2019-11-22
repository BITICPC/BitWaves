using Newtonsoft.Json;

namespace BitWaves.WebAPI.Models
{
    /// <summary>
    /// 为题目样例提供数据模型。
    /// </summary>
    public sealed class ProblemSampleTestInfo
    {
        /// <summary>
        /// 获取或设置输入数据。
        /// </summary>
        [JsonProperty("input")]
        public string Input { get; set; }

        /// <summary>
        /// 获取或设置输出数据。
        /// </summary>
        [JsonProperty("output")]
        public string Output { get; set; }
    }
}
