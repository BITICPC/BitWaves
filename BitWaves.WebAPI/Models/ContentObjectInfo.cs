using System;
using BitWaves.Data.Entities;
using BitWaves.WebAPI.Utils;
using MongoDB.Bson;
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
        /// <param name="entity">静态对象实体对象的 <see cref="BsonDocument"/> 表示。</param>
        /// <exception cref="ArgumentNullException"><paramref name="entity"/> 为 null。</exception>
        public ContentObjectInfo(BsonDocument entity)
        {
            Contract.NotNull(entity, nameof(entity));

            Id = entity["_id"].AsObjectId.ToString();
            Name = entity[nameof(Content.Name)].AsString;
            MimeType = entity[nameof(Content.MimeType)].AsString;
            CreationTime = entity[nameof(Content.CreationTime)].ToUniversalTime();
            Size = entity[nameof(Content.Size)].AsInt64;
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
