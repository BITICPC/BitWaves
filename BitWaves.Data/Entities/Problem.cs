using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BitWaves.Data.Entities
{
    /// <summary>
    /// 表示一道题目。
    /// </summary>
    public sealed class Problem
    {
        /// <summary>
        /// 获取题目实体对象 ID。
        /// </summary>
        [BsonId]
        public ObjectId Id { get; private set; }

        /// <summary>
        /// 获取或设置题目在 Problem Archive 中的 ID。若题目不在 Problem Archive 中，该属性值应为 null。
        /// </summary>
        public int? ArchiveId { get; set; }

        /// <summary>
        /// 获取或设置题目的标题。
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 获取或设置题目的作者。
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        /// 获取或设置题目的创建时间。
        /// </summary>
        public DateTime CreationTime { get; set; }

        /// <summary>
        /// 获取或设置题目的上次更改时间。
        /// </summary>
        public DateTime LastUpdateTime { get; set; }

        /// <summary>
        /// 获取或设置题目的标签列表。
        /// </summary>
        public List<string> Tags { get; set; }

        /// <summary>
        /// 获取或设置题目的难度。
        /// </summary>
        public int Difficulty { get; set; }

        /// <summary>
        /// 获取或设置题目的总提交数量。
        /// </summary>
        public int TotalSubmissions { get; set; }

        /// <summary>
        /// 获取或设置题目的总 AC 提交数量。
        /// </summary>
        public int AcceptedSubmissions { get; set; }

        /// <summary>
        /// 获取或设置题目的描述信息。
        /// </summary>
        public ProblemDescription Description { get; set; }

        /// <summary>
        /// 获取或设置题目的评测相关信息。
        /// </summary>
        public ProblemJudgeInfo JudgeInfo { get; set; }

        /// <summary>
        /// 创建 <see cref="Problem"/> 的空有效实例。
        /// </summary>
        /// <returns>新创建的 <see cref="Problem"/> 的空有效实例。</returns>
        public static Problem Create()
        {
            return new Problem
            {
                Id = ObjectId.GenerateNewId(),
                CreationTime = DateTime.UtcNow,
                LastUpdateTime = DateTime.UtcNow,
                Tags = new List<string>(),
                Description = new ProblemDescription(),
                JudgeInfo = new ProblemJudgeInfo()
            };
        }
    }
}
