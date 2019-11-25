using BitWaves.WebAPI.Validation;
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
        [AnnouncementTitle]
        public string Title { get; set; }

        /// <summary>
        /// 获取或设置创建全站公告的作者的用户名。该属性的值应该由调用方手动设置。
        /// </summary>
        [JsonIgnore]
        public string Author { get; set; }

        /// <summary>
        /// 获取或设置全站公告是否置顶标志。
        /// </summary>
        [JsonProperty("pinned")]
        public bool IsPinned { get; set; }

        /// <summary>
        /// 获取或设置全站公告内容。
        /// </summary>
        [JsonProperty("content")]
        [AnnouncementContent]
        public string Content { get; set; }
    }
}
