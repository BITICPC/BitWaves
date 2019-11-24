using System;
using System.ComponentModel.DataAnnotations;
using BitWaves.Data.Entities;
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
    }
}
