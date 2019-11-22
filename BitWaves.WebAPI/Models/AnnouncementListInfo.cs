using System;
using MongoDB.Bson;
using Newtonsoft.Json;

namespace BitWaves.WebAPI.Models
{
    /// <summary>
    /// 为全站公告提供列表数据模型。
    /// </summary>
    public class AnnouncementListInfo
    {
        /// <summary>
        /// 获取或设置全站公告的 ID。
        /// </summary>
        [JsonProperty("id")]
        public ObjectId Id { get; set; }

        /// <summary>
        /// 获取或设置全站公告的标题。
        /// </summary>
        [JsonProperty("title")]
        public string Title { get; set; }

        /// <summary>
        /// 获取或设置全站公告的作者。
        /// </summary>
        [JsonProperty("author")]
        public string Author { get; set; }

        /// <summary>
        /// 获取或设置全站公告的创建时间。
        /// </summary>
        [JsonProperty("creationTime")]
        public DateTime CreationTime { get; set; }

        /// <summary>
        /// 获取或设置全站公告的上次更新时间。
        /// </summary>
        [JsonProperty("lastUpdateTime")]
        public DateTime LastUpdateTime { get; set; }

        /// <summary>
        /// 获取或设置全站公告是否置顶。
        /// </summary>
        [JsonProperty("pinned")]
        public bool IsPinned { get; set; }
    }
}
