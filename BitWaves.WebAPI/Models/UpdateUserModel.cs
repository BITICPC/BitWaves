using System.ComponentModel.DataAnnotations;
using BitWaves.WebAPI.Utils;
using Newtonsoft.Json;

namespace BitWaves.WebAPI.Models
{
    /// <summary>
    /// 为更新用户信息提供数据模型。
    /// </summary>
    public sealed class UpdateUserModel
    {
        /// <summary>
        /// 获取手机号。
        /// </summary>
        [JsonProperty("phone")]
        [OptionalValidation(typeof(RegularExpressionAttribute), @"^\d{11}")]
        public Optional<string> Phone { get; private set; }

        /// <summary>
        /// 获取学号。
        /// </summary>
        [JsonProperty("studentId")]
        public Optional<string> StudentId { get; private set; }
    }
}
