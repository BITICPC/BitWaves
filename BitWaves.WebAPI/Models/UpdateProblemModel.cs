using BitWaves.Data.Entities;
using BitWaves.Data.Utils;
using BitWaves.WebAPI.Validation;
using Newtonsoft.Json;

namespace BitWaves.WebAPI.Models
{
    /// <summary>
    /// 为更新题目信息操作提供数据模型。
    /// </summary>
    public sealed class UpdateProblemModel
    {
        /// <summary>
        /// 获取题目的标题。
        /// </summary>
        [JsonProperty("title")]
        [Inner(typeof(ProblemTitleAttribute))]
        public Maybe<string> Title { get; private set; }

        /// <summary>
        /// 获取题目的背景描述。
        /// </summary>
        [JsonProperty("legend")]
        public Maybe<string> Legend { get; private set; }

        /// <summary>
        /// 获取题目的输入格式描述。
        /// </summary>
        [JsonProperty("input")]
        public Maybe<string> Input { get; private set; }

        /// <summary>
        /// 获取题目的输出格式描述。
        /// </summary>
        [JsonProperty("output")]
        public Maybe<string> Output { get; private set; }

        /// <summary>
        /// 获取题目的提示信息。
        /// </summary>
        [JsonProperty("notes")]
        public Maybe<string> Notes { get; private set; }

        /// <summary>
        /// 获取题目的难度系数。
        /// </summary>
        [JsonProperty("difficulty")]
        [Inner(typeof(DifficultyAttribute))]
        public Maybe<int> Difficulty { get; private set; }

        /// <summary>
        /// 获取题目单个测试点的时间限制，单位为毫秒。
        /// </summary>
        [JsonProperty("timeLimit")]
        [Inner(typeof(TimeLimitAttribute))]
        public Maybe<int> TimeLimit { get; private set; }

        /// <summary>
        /// 获取题目单个测试点的内存限制，单位为 MB。
        /// </summary>
        [JsonProperty("memoryLimit")]
        [Inner(typeof(MemoryLimitAttribute))]
        public Maybe<int> MemoryLimit { get; private set; }

        /// <summary>
        /// 获取题目的评测模式。
        /// </summary>
        [JsonProperty("judgeMode")]
        public Maybe<ProblemJudgeMode> JudgeMode { get; private set; }

        /// <summary>
        /// 当评测模式为 Standard 时，获取传递给内建答案检查器的选项。
        /// </summary>
        [JsonProperty("builtinCheckerOptions")]
        public Maybe<BuiltinCheckerOptions> BuiltinCheckerOptions { get; private set; }
    }
}
