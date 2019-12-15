using BitWaves.Data.Entities;
using BitWaves.WebAPI.Validation;
using MongoDB.Bson;
using Newtonsoft.Json;

namespace BitWaves.WebAPI.Models
{
    /// <summary>
    /// 为创建提交操作提供数据模型。
    /// </summary>
    public sealed class CreateSubmissionModel
    {
        /// <summary>
        /// 获取或设置提交的题目的全局唯一 ID。
        /// </summary>
        [JsonProperty("problemId")]
        public ObjectId ProblemId { get; set; }

        /// <summary>
        /// 获取或设置提交的语言的全局唯一 ID。
        /// </summary>
        [JsonProperty("languageId")]
        public ObjectId LanguageId { get; set; }

        /// <summary>
        /// 获取或设置创建提交的用户的用户名。该属性应该由调用方设置。
        /// </summary>
        [JsonIgnore]
        public string Author { get; set; }

        /// <summary>
        /// 获取或设置提交的语言的三元组。该属性应该由调用方设置。
        /// </summary>
        [JsonIgnore]
        public LanguageTriple LanguageTriple { get; set; }

        /// <summary>
        /// 获取或设置提交的语言的显示名称。该属性应该由调用方设置。
        /// </summary>
        [JsonIgnore]
        public string LanguageDisplayName { get; set; }

        /// <summary>
        /// 获取或设置提交的源代码。
        /// </summary>
        [JsonProperty("code")]
        [Code]
        public string Code { get; set; }
    }
}
