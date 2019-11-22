using System;
using System.Collections.Generic;
using MongoDB.Bson;
using Newtonsoft.Json;

namespace BitWaves.WebAPI.Models
{
    public class ProblemListInfo
    {
        /// <summary>
        /// 获取或设置题目 ID。
        /// </summary>
        [JsonProperty("id")]
        public ObjectId Id { get; set; }

        /// <summary>
        /// 获取或设置题目在 Archive 中的 ID。
        /// </summary>
        [JsonProperty("archiveId")]
        public int? ArchiveId { get; set; }

        /// <summary>
        /// 获取或设置题目标题。
        /// </summary>
        [JsonProperty("title")]
        public string Title { get; set; }

        /// <summary>
        /// 获取或设置题目的创建时间。
        /// </summary>
        [JsonProperty("creationTime")]
        public DateTime CreationTime { get; set; }

        /// <summary>
        /// 获取或设置题目的上次更新时间。
        /// </summary>
        [JsonProperty("lastUpdateTime")]
        public DateTime LastUpdateTime { get; set; }

        /// <summary>
        /// 获取或设置题目的作者。
        /// </summary>
        [JsonProperty("author")]
        public string Author { get; set; }

        /// <summary>
        /// 获取或设置题目的来源。
        /// </summary>
        [JsonProperty("source")]
        public string Source { get; set; }

        /// <summary>
        /// 获取或设置题目的难度系数。
        /// </summary>
        [JsonProperty("difficulty")]
        public int Difficulty { get; set; }

        /// <summary>
        /// 获取或设置题目的标签。
        /// </summary>
        [JsonProperty("tags")]
        public List<string> Tags { get; set; }

        /// <summary>
        /// 获取或设置题目的总提交数量。
        /// </summary>
        [JsonProperty("totalSubmissions")]
        public int TotalSubmissions { get; set; }

        /// <summary>
        /// 获取或设置题目的 AC 提交数量。
        /// </summary>
        [JsonProperty("acceptedSubmissions")]
        public int AcceptedSubmissions { get; set; }

        /// <summary>
        /// 获取或设置尝试过该题目的用户数量。
        /// </summary>
        [JsonProperty("totalAttemptedUsers")]
        public int TotalAttemptedUsers { get; set; }

        /// <summary>
        /// 获取或设置成功解答该题目的用户数量。
        /// </summary>
        [JsonProperty("totalSolvedUsers")]
        public int TotalSolvedUsers { get; set; }

        /// <summary>
        /// 获取或设置上次提交本题目的时间。
        /// </summary>
        [JsonProperty("lastSubmissionTime")]
        public DateTime LastSubmissionTime { get; set; }
    }
}
