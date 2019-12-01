using BitWaves.Data.Utils;
using BitWaves.WebAPI.Validation;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
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
        [Inner(typeof(AnnouncementTitleAttribute))]
        public Maybe<string> Title { get; set; }

        /// <summary>
        /// 获取或设置全站公告的置顶标志位。
        /// </summary>
        [JsonProperty("pinned")]
        [ValidateNever]
        public Maybe<bool> IsPinned { get; set; }

        /// <summary>
        /// 获取或设置全站公告的内容。
        /// </summary>
        [JsonProperty("content")]
        [Inner(typeof(AnnouncementContentAttribute))]
        public Maybe<string> Content { get; set; }
    }
}
