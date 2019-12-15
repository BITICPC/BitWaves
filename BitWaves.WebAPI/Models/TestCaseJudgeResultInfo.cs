using BitWaves.Data.Entities;
using Newtonsoft.Json;

namespace BitWaves.WebAPI.Models
{
    /// <summary>
    /// 为单个测试用例的评测结果提供数据模型。
    /// </summary>
    public sealed class TestCaseJudgeResultInfo
    {
        /// <summary>
        /// 获取或设置测试用例的评测结果。
        /// </summary>
        [JsonProperty("verdict")]
        public Verdict Verdict { get; set; }

        /// <summary>
        /// 获取或设置提交的程序消耗的 CPU 时间，单位为毫秒。
        /// </summary>
        [JsonProperty("time")]
        public int Time { get; set; }

        /// <summary>
        /// 获取或设置提交的程序的内存占用，单位为 MB。
        /// </summary>
        [JsonProperty("memory")]
        public int Memory { get; set; }

        /// <summary>
        /// 获取或设置提交的程序的退出代码。
        /// </summary>
        [JsonProperty("exitCode")]
        public int ExitCode { get; set; }

        /// <summary>
        /// 获取或设置输入数据的部分视图。
        /// </summary>
        [JsonProperty("inputView")]
        public string InputView { get; set; }

        /// <summary>
        /// 获取或设置答案数据的部分视图。
        /// </summary>
        [JsonProperty("answerView")]
        public string AnswerView { get; set; }

        /// <summary>
        /// 获取或设置提交的程序的输出数据的部分视图。
        /// </summary>
        [JsonProperty("outputView")]
        public string OutputView { get; set; }

        /// <summary>
        /// 获取或设置评测系统产生的消息。
        /// </summary>
        [JsonProperty("comment")]
        public string Comment { get; set; }
    }
}
