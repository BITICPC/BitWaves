using System;
using BitWaves.Data.Entities;
using BitWaves.WebAPI.Utils;
using Newtonsoft.Json;

namespace BitWaves.WebAPI.Models
{
    /// <summary>
    /// 为语言信息提供数据模型。
    /// </summary>
    public sealed class LanguageInfo
    {
        /// <summary>
        /// 初始化 <see cref="LanguageInfo"/> 的新实例。
        /// </summary>
        /// <param name="entity">语言实体对象.</param>
        /// <exception cref="ArgumentNullException"><paramref name="entity"/> 为 null。</exception>
        public LanguageInfo(Language entity)
        {
            Contract.NotNull(entity, nameof(entity));

            Id = entity.Id.ToString();
            Identifier = entity.Identifier;
            Dialect = entity.Dialect;
            Version = entity.Version;
            DisplayName = entity.DisplayName;
        }

        /// <summary>
        /// 获取语言的全局唯一 ID。
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; }

        /// <summary>
        /// 获取语言的标识符。
        /// </summary>
        [JsonProperty("langId")]
        public string Identifier { get; }

        /// <summary>
        /// 获取语言的方言标识符。
        /// </summary>
        [JsonProperty("dialect")]
        public string Dialect { get; }

        /// <summary>
        /// 获取语言的版本标识符。
        /// </summary>
        [JsonProperty("version")]
        public string Version { get; }

        /// <summary>
        /// 获取语言的显示名称。
        /// </summary>
        [JsonProperty("displayName")]
        public string DisplayName { get; }
    }
}
