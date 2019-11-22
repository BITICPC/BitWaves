using Newtonsoft.Json;

namespace BitWaves.WebAPI.Models
{
    /// <summary>
    /// 为语言信息提供数据模型。
    /// </summary>
    public sealed class LanguageInfo
    {
        /// <summary>
        /// 获取或设置语言的全局唯一 ID。
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// 获取或设置语言的标识符。
        /// </summary>
        [JsonProperty("langId")]
        public string Identifier { get; set; }

        /// <summary>
        /// 获取或设置语言的方言标识符。
        /// </summary>
        [JsonProperty("dialect")]
        public string Dialect { get; set; }

        /// <summary>
        /// 获取或设置语言的版本标识符。
        /// </summary>
        [JsonProperty("version")]
        public string Version { get; set; }

        /// <summary>
        /// 获取或设置语言的显示名称。
        /// </summary>
        [JsonProperty("displayName")]
        public string DisplayName { get; set; }
    }
}
