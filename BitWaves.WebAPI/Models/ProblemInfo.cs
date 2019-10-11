using System;
using BitWaves.Data.Entities;
using BitWaves.WebAPI.Utils;
using Newtonsoft.Json;

namespace BitWaves.WebAPI.Models
{
    /// <summary>
    /// 为题目信息提供数据模型。
    /// </summary>
    public sealed class ProblemInfo
    {
        /// <summary>
        /// 初始化 <see cref="ProblemInfo"/> 类的新实例。
        /// </summary>
        /// <param name="entity">题目实体对象。</param>
        /// <exception cref="ArgumentNullException"><paramref name="entity"/> 为 null。</exception>
        public ProblemInfo(Problem entity)
        {
            Contract.NotNull(entity, nameof(entity));

            Id = entity.Id.ToString();
            ArchiveId = entity.ArchiveId;
            Title = entity.Title;
            CreationTime = entity.CreationTime;
            LastUpdateTime = entity.LastUpdateTime;
            Author = entity.Author;
            Difficulty = entity.Difficulty;
            Tags = entity.Tags.ToArray();
            TotalSubmissions = entity.TotalSubmissions;
            AcceptedSubmissions = entity.AcceptedSubmissions;
        }

        /// <summary>
        /// 获取题目 ID。
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; }

        /// <summary>
        /// 获取题目在 Archive 中的 ID。
        /// </summary>
        [JsonProperty("archiveId")]
        public int? ArchiveId { get; }

        /// <summary>
        /// 获取题目标题。
        /// </summary>
        [JsonProperty("title")]
        public string Title { get; }

        /// <summary>
        /// 获取题目的创建时间。
        /// </summary>
        [JsonProperty("creationTime")]
        public DateTime CreationTime { get; }

        /// <summary>
        /// 获取题目的上次更新时间。
        /// </summary>
        [JsonProperty("lastUpdateTime")]
        public DateTime LastUpdateTime { get; }

        /// <summary>
        /// 获取题目的作者。
        /// </summary>
        [JsonProperty("author")]
        public string Author { get; }

        /// <summary>
        /// 获取题目的难度系数。
        /// </summary>
        [JsonProperty("difficulty")]
        public int Difficulty { get; }

        /// <summary>
        /// 获取题目的标签。
        /// </summary>
        [JsonProperty("tags")]
        public string[] Tags { get; }

        /// <summary>
        /// 获取题目的总提交数量。
        /// </summary>
        [JsonProperty("totalSubmissions")]
        public int TotalSubmissions { get; }

        /// <summary>
        /// 获取题目的 AC 提交数量。
        /// </summary>
        [JsonProperty("acceptedSubmissions")]
        public int AcceptedSubmissions { get; }
    }
}