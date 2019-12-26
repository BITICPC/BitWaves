using Newtonsoft.Json;

namespace BitWaves.WebAPI.Models
{
    /// <summary>
    /// 为提交详细信息提供数据模型。
    /// </summary>
    public sealed class SubmissionInfo : SubmissionListInfo
    {
        /// <summary>
        /// 获取或设置提交的源代码。
        /// </summary>
        [JsonProperty("code")]
        public string Code { get; set; }

        /// <summary>
        /// 获取或设置提交的评测结果。
        /// </summary>
        [JsonProperty("judgeResult")]
        public SubmissionJudgeResultInfo Result { get; set; }
    }
}
