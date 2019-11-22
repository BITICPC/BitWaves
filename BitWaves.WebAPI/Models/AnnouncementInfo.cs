using System;
using BitWaves.Data.Entities;
using Newtonsoft.Json;

namespace BitWaves.WebAPI.Models
{
    /// <summary>
    /// 为全站公告提供数据模型。
    /// </summary>
    public sealed class AnnouncementInfo
    {
        /// <summary>
        /// 初始化 <see cref="AnnouncementInfo"/> 类的新实例。
        /// </summary>
        /// <param name="announcement">全站公告实体对象。</param>
        /// <param name="scheme">全站公告数据模型的使用场景。</param>
        /// <exception cref="ArgumentNullException"><paramref name="announcement"/> 为 null。</exception>
        public AnnouncementInfo(Announcement announcement, AnnouncementInfoScheme scheme)
        {
            Contract.NotNull(announcement, nameof(announcement));

            Id = announcement.Id.ToString();
            Title = announcement.Title;
            Author = announcement.Author;
            CreationTime = announcement.CreationTime;
            LastUpdateTime = announcement.LastUpdateTime;
            IsPinned = announcement.IsPinned;
            Content = announcement.Content;

            Scheme = scheme;
        }

        /// <summary>
        /// 获取全站公告的 ID。
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; }

        /// <summary>
        /// 获取全站公告的标题。
        /// </summary>
        [JsonProperty("title")]
        public string Title { get; }

        /// <summary>
        /// 获取全站公告的作者。
        /// </summary>
        [JsonProperty("author")]
        public string Author { get; }

        /// <summary>
        /// 获取全站公告的创建时间。
        /// </summary>
        [JsonProperty("creationTime")]
        public DateTime CreationTime { get; }

        /// <summary>
        /// 获取全站公告的上次更新时间。
        /// </summary>
        [JsonProperty("lastUpdateTime")]
        public DateTime LastUpdateTime { get; }

        /// <summary>
        /// 获取全站公告是否置顶。
        /// </summary>
        [JsonProperty("pinned")]
        public bool IsPinned { get; }

        /// <summary>
        /// 获取全站公告的内容。
        /// </summary>
        [JsonProperty("content")]
        public string Content { get; }

        /// <summary>
        /// 获取全站公告的使用场景。
        /// </summary>
        [JsonIgnore]
        public AnnouncementInfoScheme Scheme { get; }

        public bool ShouldSerializeContent()
        {
            return Scheme == AnnouncementInfoScheme.Full;
        }
    }
}
