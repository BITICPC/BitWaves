using System;
using MongoDB.Bson;
using Newtonsoft.Json;

namespace BitWaves.WebAPI.Models
{
    /// <summary>
    /// 为静态对象的元信息提供数据模型。
    /// </summary>
    public sealed class ContentInfo
    {
        /// <summary>
        /// 获取或设置静态对象的全局唯一 ID。
        /// </summary>
        [JsonProperty("id")]
        public ObjectId Id { get; set; }

        /// <summary>
        /// 获取或设置静态对象的名称。
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// 获取或设置静态对象的 MIME 类型。
        /// </summary>
        [JsonProperty("mimeType")]
        public string MimeType { get; set; }

        /// <summary>
        /// 获取或设置静态对象的创建时间。
        /// </summary>
        [JsonProperty("creationTime")]
        public DateTime CreationTime { get; set; }

        /// <summary>
        /// 获取或设置静态对象的字节大小。
        /// </summary>
        [JsonProperty("size")]
        public long Size { get; set; }
    }
}
