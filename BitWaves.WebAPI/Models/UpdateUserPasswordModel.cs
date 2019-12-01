using BitWaves.Data.Utils;
using BitWaves.WebAPI.Validation;
using Newtonsoft.Json;

namespace BitWaves.WebAPI.Models
{
    /// <summary>
    /// 为更新用户密码操作提供数据模型。
    /// </summary>
    public sealed class UpdateUserPasswordModel
    {
        /// <summary>
        /// 获取或设置旧密码。
        /// </summary>
        [JsonProperty("oldPassword")]
        [Inner(typeof(PasswordAttribute))]
        public Maybe<string> OldPassword { get; set; }

        /// <summary>
        /// 获取新密码。
        /// </summary>
        [JsonProperty("password")]
        [Password]
        public string NewPassword { get; set; }
    }
}
