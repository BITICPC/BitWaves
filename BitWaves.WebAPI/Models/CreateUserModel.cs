using BitWaves.WebAPI.Validation;
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
        [Username]
        public string Username { get; private set; }

        /// <summary>
        /// 获取用户密码。
        /// </summary>
        [JsonProperty("password")]
        [Password]
        public string Password { get; private set; }

        /// <summary>
        /// 获取用户手机号。
        /// </summary>
        [JsonProperty("phone")]
        [PhoneNumber]
        public string Phone { get; private set; }
    }
}
