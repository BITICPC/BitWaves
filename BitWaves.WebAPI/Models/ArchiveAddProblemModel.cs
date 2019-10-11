using System.ComponentModel.DataAnnotations;
using BitWaves.Data.Entities;
using MongoDB.Bson;
using MongoDB.Driver;
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
        [Range(0, int.MaxValue)]
        public int ArchiveId { get; private set; }

        /// <summary>
        /// 从当前的数据模型创建更新数据库所需的 <see cref="FilterDefinition{Problem}"/> 以及
        /// <see cref="UpdateDefinition{Problem}"/> 定义。
        /// </summary>
        /// <returns>二元组，第一项表示目标实体对象的筛选定义，第二项表示目标实体对象的更新定义。</returns>
        public (FilterDefinition<Problem> Filter, UpdateDefinition<Problem> Update) CreateUpdateDefinition()
        {
            return (Builders<Problem>.Filter.Eq(p => p.Id, ProblemId),
                Builders<Problem>.Update.Set(p => p.ArchiveId, ArchiveId));
        }
    }
}
