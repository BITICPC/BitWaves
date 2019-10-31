using System;
using BitWaves.Data.Entities;
using BitWaves.WebAPI.Utils;
using Newtonsoft.Json;

namespace BitWaves.WebAPI.Models
{
    /// <summary>
    /// 为静态对象的元信息提供数据模型。
    /// </summary>
    public sealed class ContentObjectInfo
    {
        /// <summary>
        /// 初始化 <see cref="ContentObjectInfo"/> 类的新实例。
        /// </summary>
        /// <param name="entity">静态对象实体对象。</param>
        /// <exception cref="ArgumentNullException"><paramref name="entity"/> 为 null。</exception>
        public ContentObjectInfo(Content entity)
        {
            Contract.NotNull(entity, nameof(entity));

            Id = entity.Id.ToString();
            Name = entity.Name;
            MimeType = entity.MimeType;
            CreationTime = entity.CreationTime;
            Size = entity.Size;
        }

        /// <summary>
        /// 获取静态对象的全局唯一 ID。
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; }

        /// <summary>
        /// 获取静态对象的名称。
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; }

        /// <summary>
        /// 获取静态对象的 MIME 类型。
        /// </summary>
        [JsonProperty("mimeType")]
        public string MimeType { get; }

        /// <summary>
        /// 获取静态对象的创建时间。
        /// </summary>
        [JsonProperty("creationTime")]
        public DateTime CreationTime { get; }

        /// <summary>
        /// 获取静态对象的字节大小。
        /// </summary>
        [JsonProperty("size")]
        public long Size { get; }
    }
}
