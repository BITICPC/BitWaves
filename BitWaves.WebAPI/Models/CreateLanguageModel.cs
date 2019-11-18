using System.ComponentModel.DataAnnotations;
using BitWaves.Data.Entities;
using Newtonsoft.Json;

namespace BitWaves.WebAPI.Models
{
    /// <summary>
    /// 为创建语言操作提供数据模型。
    /// </summary>
    public sealed class CreateLanguageModel
    {
        /// <summary>
        /// 获取或设置语言标识符。
        /// </summary>
        [JsonProperty("langId")]
        [Required]
        [MinLength(1)]
        public string LanguageIdentifier { get; set; }

        /// <summary>
        /// 获取或设置语言方言。
        /// </summary>
        [JsonProperty("dialect")]
        [Required]
        [MinLength(1)]
        public string Dialect { get; set; }

        /// <summary>
        /// 获取或设置语言版本。
        /// </summary>
        [JsonProperty("version")]
        [Required]
        [MinLength(1)]
        public string Version { get; set; }

        /// <summary>
        /// 获取或设置语言的显示名称。
        /// </summary>
        [JsonProperty("displayName")]
        [Required]
        [MinLength(1)]
        public string DisplayName { get; set; }

        /// <summary>
        /// 从当前的数据模型对象创建 <see cref="Language"/> 实体对象。
        /// </summary>
        /// <returns>创建的 <see cref="Language"/> 实体对象。</returns>
        public Language ToEntity()
        {
            return Language.Create(LanguageIdentifier, Dialect, Version, DisplayName);
        }
    }
}
