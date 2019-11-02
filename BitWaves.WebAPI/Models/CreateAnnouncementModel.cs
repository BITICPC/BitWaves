using System;
using System.ComponentModel.DataAnnotations;
using BitWaves.Data.Entities;
using BitWaves.WebAPI.Utils;
using Newtonsoft.Json;

namespace BitWaves.WebAPI.Models
{
    /// <summary>
    /// 为全站公告的创建提供数据模型。
    /// </summary>
    public sealed class CreateAnnouncementModel
    {
        /// <summary>
        /// 获取或设置全站公告的标题。
        /// </summary>
        [JsonProperty("title")]
        [Required]
        public string Title { get; set; }

        /// <summary>
        /// 获取或设置全站公告是否置顶标志。
        /// </summary>
        [JsonProperty("pinned")]
        public bool IsPinned { get; set; }

        /// <summary>
        /// 获取或设置全站公告内容。
        /// </summary>
        [JsonProperty("content")]
        [Required]
        public string Content { get; set; }

        /// <summary>
        /// 从当前的数据模型对象创建 <see cref="Announcement"/> 实体对象。
        /// </summary>
        /// <param name="author">发布全站公告的用户。</param>
        /// <exception cref="ArgumentNullException"><paramref name="author"/> 为 null。</exception>
        /// <returns>新创建的 <see cref="Announcement"/> 实体对象。</returns>
        public Announcement ToAnnouncementEntity(string author)
        {
            Contract.NotNull(author, nameof(author));

            var announcement = Announcement.Create(author, Title, Content);
            announcement.IsPinned = IsPinned;

            return announcement;
        }
    }
}
