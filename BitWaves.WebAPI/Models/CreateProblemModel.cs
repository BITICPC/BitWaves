using BitWaves.WebAPI.Validation;
using Newtonsoft.Json;

namespace BitWaves.WebAPI.Models
{
    /// <summary>
    /// 为创建题目操作提供数据模型。
    /// </summary>
    public sealed class CreateProblemModel
    {
        /// <summary>
        /// 获取或设置题目标题。
        /// </summary>
        [JsonProperty("title")]
        [ProblemTitle]
        public string Title { get; set; }
    }
}
