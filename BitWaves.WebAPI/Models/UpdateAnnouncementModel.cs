using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BitWaves.Data.Entities;
using BitWaves.WebAPI.Utils;
using MongoDB.Driver;
using Newtonsoft.Json;

namespace BitWaves.WebAPI.Models
{
    /// <summary>
    /// 为全站公告的更新提供数据模型。
    /// </summary>
    public sealed class UpdateAnnouncementModel
    {
        /// <summary>
        /// 获取或设置全站公告的标题。
        /// </summary>
        [JsonProperty("title")]
        [OptionalValidation(typeof(RequiredAttribute))]
        public Utils.Optional<string> Title { get; set; }

        /// <summary>
        /// 获取或设置全站公告的置顶标志位。
        /// </summary>
        [JsonProperty("pinned")]
        public Utils.Optional<bool> IsPinned { get; set; }

        /// <summary>
        /// 获取或设置全站公告的内容。
        /// </summary>
        [JsonProperty("content")]
        [OptionalValidation(typeof(RequiredAttribute))]
        public Utils.Optional<string> Content { get; set; }

        /// <summary>
        /// 从当前的数据模型获取对 <see cref="Announcement"/> 实体对象的更新定义。
        /// </summary>
        /// <returns>对 <see cref="Announcement"/> 实体对象的更新定义。</returns>
        public UpdateDefinition<Announcement> GetUpdateDefinition()
        {
            var updates = new List<UpdateDefinition<Announcement>>();

            if (Title.HasValue)
            {
                updates.Add(Builders<Announcement>.Update.Set(ann => ann.Title, Title.Value));
            }

            if (IsPinned.HasValue)
            {
                updates.Add(Builders<Announcement>.Update.Set(ann => ann.IsPinned, IsPinned.Value));
            }

            if (Content.HasValue)
            {
                updates.Add(Builders<Announcement>.Update.Set(ann => ann.Content, Content.Value));
            }

            return Builders<Announcement>.Update.Combine(updates);
        }
    }
}
