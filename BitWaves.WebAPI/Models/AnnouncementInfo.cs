using Newtonsoft.Json;

namespace BitWaves.WebAPI.Models
{
    /// <summary>
    /// 为全站公告提供详细数据模型。
    /// </summary>
    public class AnnouncementInfo : AnnouncementListInfo
    {
        /// <summary>
        /// 获取全站公告的内容。
        /// </summary>
        [JsonProperty("content")]
        public string Content { get; }
    }
}
