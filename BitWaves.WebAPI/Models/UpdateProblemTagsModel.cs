using System.Collections.Generic;
using BitWaves.Data.Entities;
using BitWaves.WebAPI.Validation;
using MongoDB.Driver;

namespace BitWaves.WebAPI.Models
{
    /// <summary>
    /// 为更新题目标签提供数据模型。
    /// </summary>
    public sealed class UpdateProblemTagsModel
    {
        /// <summary>
        /// 获取要添加的题目标签。
        /// </summary>
        [OptionalValidation(typeof(EnumerableValidation), typeof(ProblemTagNameAttribute))]
        public Utils.Optional<List<string>> TagsToAdd { get; private set; }

        /// <summary>
        /// 获取要删除的题目标签。
        /// </summary>
        [OptionalValidation(typeof(EnumerableValidation), typeof(ProblemTagNameAttribute))]
        public Utils.Optional<List<string>> TagsToRemove { get; private set; }

        /// <summary>
        /// 从当前的数据模型创建相应的 <see cref="UpdateDefinition{Problem}"/> 对象。
        /// </summary>
        /// <returns>
        /// 从当前的数据模型创建的 <see cref="UpdateDefinition{Problem}"/> 对象。若没有任何数据需要更新，返回 null。
        /// </returns>
        public UpdateDefinition<Problem> ToUpdateDefinition()
        {
            var updates = new List<UpdateDefinition<Problem>>();

            if (TagsToAdd.HasValue)
            {
                updates.Add(Builders<Problem>.Update.AddToSetEach(p => p.Tags, TagsToAdd.Value));
            }

            if (TagsToRemove.HasValue)
            {
                updates.Add(Builders<Problem>.Update.PullAll(p => p.Tags, TagsToRemove.Value));
            }

            return updates.Count > 0
                ? Builders<Problem>.Update.Combine(updates)
                : null;
        }
    }
}
