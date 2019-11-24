using BitWaves.WebAPI.Validation;
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
        [Username]
        public string Username { get; private set; }

        /// <summary>
        /// 获取登录密码。
        /// </summary>
        [JsonProperty("password")]
        [Password]
        public string Password { get; private set; }
    }
}
