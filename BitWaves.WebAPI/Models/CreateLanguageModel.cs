using BitWaves.WebAPI.Validation;
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
        [LanguageIdentifier]
        public string Identifier { get; set; }

        /// <summary>
        /// 获取或设置语言方言。
        /// </summary>
        [JsonProperty("dialect")]
        [LanguageDialect]
        public string Dialect { get; set; }

        /// <summary>
        /// 获取或设置语言版本。
        /// </summary>
        [JsonProperty("version")]
        [LanguageVersion]
        public string Version { get; set; }

        /// <summary>
        /// 获取或设置语言的显示名称。
        /// </summary>
        [JsonProperty("displayName")]
        [LanguageDisplayName]
        public string DisplayName { get; set; }
    }
}
