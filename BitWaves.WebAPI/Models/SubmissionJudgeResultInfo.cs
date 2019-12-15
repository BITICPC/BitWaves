using System.Collections.Generic;
using BitWaves.Data.Entities;
using Newtonsoft.Json;

namespace BitWaves.WebAPI.Models
{
    /// <summary>
    /// 为评测结果提供数据模型。
    /// </summary>
    public sealed class SubmissionJudgeResultInfo
    {
        /// <summary>
        /// 获取或设置评测结果。
        /// </summary>
        [JsonProperty("verdict")]
        public Verdict Verdict { get; set; }

        /// <summary>
        /// 获取或设置提交的程序在其运行的所有测试用例上所消耗的 CPU 时间的最大值，单位为毫秒。
        /// </summary>
        [JsonProperty("time")]
        public int Time { get; set; }

        /// <summary>
        /// 获取或设置提交的程序在其运行的所有测试用例上消耗的内存的最大值，单位为 MB。
        /// </summary>
        [JsonProperty("memory")]
        public int Memory { get; set; }

        /// <summary>
        /// 获取或设置提交在每一个运行的测试用例上的评测结果。
        /// </summary>
        [JsonProperty("testCases")]
        public List<TestCaseJudgeResultInfo> TestCaseResults { get; set; }
    }
}
