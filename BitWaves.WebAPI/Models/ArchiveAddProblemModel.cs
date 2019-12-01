using BitWaves.WebAPI.Validation;
using MongoDB.Bson;
using Newtonsoft.Json;

namespace BitWaves.WebAPI.Models
{
    /// <summary>
    /// 为向公开题目集中添加题目操作提供数据模型。
    /// </summary>
    public sealed class ArchiveAddProblemModel
    {
        /// <summary>
        /// 获取题目的全局唯一 ID。
        /// </summary>
        [JsonProperty("id")]
        public ObjectId ProblemId { get; private set; }

        /// <summary>
        /// 获取题目在公开题目集中的 ID。
        /// </summary>
        [JsonProperty("archiveId")]
        [ArchiveId]
        public int ArchiveId { get; private set; }
    }
}
