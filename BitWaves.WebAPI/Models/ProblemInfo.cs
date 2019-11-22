using System.Collections.Generic;
using BitWaves.Data.Entities;
using Newtonsoft.Json;

namespace BitWaves.WebAPI.Models
{
    /// <summary>
    /// 为题目信息提供数据模型。
    /// </summary>
    public class ProblemInfo : ProblemListInfo
    {
        /// <summary>
        /// 获取或设置题目的正文叙述。
        /// </summary>
        [JsonProperty("legend")]
        public string Legend { get; set; }

        /// <summary>
        /// 获取或设置题目的输入描述。
        /// </summary>
        [JsonProperty("input")]
        public string Input { get; set; }

        /// <summary>
        /// 获取或设置题目的输出描述。
        /// </summary>
        [JsonProperty("output")]
        public string Output { get; set; }

        /// <summary>
        /// 获取或设置题目的测试样例。
        /// </summary>
        [JsonProperty("samples")]
        public List<ProblemSampleTestInfo> SampleTests { get; set; }

        /// <summary>
        /// 获取或设置题目的提示信息。
        /// </summary>
        [JsonProperty("notes")]
        public string Notes { get; set; }

        /// <summary>
        /// 获取或设置题目的时间限制，单位为毫秒。
        /// </summary>
        [JsonProperty("timeLimit")]
        public int TimeLimit { get; set; }

        /// <summary>
        /// 获取或设置题目的空间限制，单位为 MB。
        /// </summary>
        [JsonProperty("memoryLimit")]
        public int MemoryLimit { get; set; }

        /// <summary>
        /// 获取或设置题目的评测模式。
        /// </summary>
        [JsonProperty("judgeMode")]
        public ProblemJudgeMode JudgeMode { get; set; }

        /// <summary>
        /// 当评测模式为 Standard 时，获取或设置内建答案检查器的选项列表。
        /// </summary>
        [JsonProperty("builtinCheckerOptions")]
        public BuiltinCheckerOptions? BuiltinCheckerOptions { get; set; }

        /// <summary>
        /// 获取或设置题目是否包含有效的测试数据集。
        /// </summary>
        [JsonProperty("testReady")]
        public bool IsTestReady { get; set; }
    }
}
