using System;
using BitWaves.Data.Entities;
using MongoDB.Bson;
using Newtonsoft.Json;

namespace BitWaves.WebAPI.Models
{
    /// <summary>
    /// 为提交列表中的提交信息提供数据模型。
    /// </summary>
    public class SubmissionListInfo
    {
        /// <summary>
        /// 获取或设置提交的 ID。
        /// </summary>
        [JsonProperty("id")]
        public ObjectId Id { get; set; }

        /// <summary>
        /// 获取或设置创建提交的用户名。
        /// </summary>
        [JsonProperty("author")]
        public string Author { get; set; }

        /// <summary>
        /// 获取或设置提交的题目 ID。
        /// </summary>
        [JsonProperty("problemId")]
        public ObjectId ProblemId { get; set; }

        /// <summary>
        /// 获取或设置提交的题目在公开题目集中的 ID。
        /// </summary>
        [JsonProperty("problemArchiveId")]
        public int? ProblemArchiveId { get; set; }

        /// <summary>
        /// 获取或设置提交的题目的标题。
        /// </summary>
        [JsonProperty("problemTitle")]
        public string ProblemTitle { get; set; }

        /// <summary>
        /// 获取或设置提交的创建时间。
        /// </summary>
        [JsonProperty("creationTime")]
        public DateTime CreationTime { get; set; }

        /// <summary>
        /// 获取或设置提交的评测时间。若该提交尚未被评测，该字段为 null。
        /// </summary>
        [JsonProperty("judgeTime")]
        public DateTime? JudgeTime { get; set; }

        /// <summary>
        /// 获取或设置提交的语言的显示名称。
        /// </summary>
        [JsonProperty("language")]
        public string Language { get; set; }

        /// <summary>
        /// 获取或设置提交的评测状态。
        /// </summary>
        [JsonProperty("status")]
        public JudgeStatus Status { get; set; }

        /// <summary>
        /// 获取或设置提交的评测结果。
        /// </summary>
        [JsonProperty("verdict")]
        public Verdict? Verdict { get; set; }

        /// <summary>
        /// 获取或设置用户提交消耗的的 CPU 时间，单位为毫秒。
        /// </summary>
        [JsonProperty("time")]
        public int? Time { get; set; }

        /// <summary>
        /// 获取或设置用户提交的峰值内存占用，单位为 MB。
        /// </summary>
        [JsonProperty("memory")]
        public int? Memory { get; set; }
    }
}
