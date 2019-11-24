using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace BitWaves.WebAPI.Models
{
    /// <summary>
    /// 为创建用户操作提供数据模型。
    /// </summary>
    public sealed class CreateUserModel
    {
        /// <summary>
        /// 获取用户名。
        /// </summary>
        [JsonProperty("username")]
        [Required]
        [MinLength(3)]
        public string Username { get; private set; }

        /// <summary>
        /// 获取用户密码。
        /// </summary>
        [JsonProperty("password")]
        [Required]
        [MinLength(6)]
        public string Password { get; private set; }

        /// <summary>
        /// 获取用户手机号。
        /// </summary>
        [JsonProperty("phone")]
        [Required]
        [StringLength(11)]
        [RegularExpression(@"^\d{11}")]
        public string Phone { get; private set; }
    }
}
