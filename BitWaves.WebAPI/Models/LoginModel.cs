using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace BitWaves.WebAPI.Models
{
    /// <summary>
    /// 为用户登录提供数据模型。
    /// </summary>
    public sealed class LoginModel
    {
        /// <summary>
        /// 获取用户名。
        /// </summary>
        [JsonProperty("username")]
        [Required(ErrorMessage = "用户名未填写")]
        [MinLength(3, ErrorMessage = "用户名长度至少为3个字符")]
        public string Username { get; private set; }

        /// <summary>
        /// 获取登录密码。
        /// </summary>
        [JsonProperty("password")]
        [Required(ErrorMessage = "密码未填写")]
        [MinLength(6, ErrorMessage = "密码长度至少为6个字符")]
        public string Password { get; private set; }
    }
}
