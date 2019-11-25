using BitWaves.Data;
using BitWaves.Data.Entities;
using BitWaves.WebAPI.Validation;
using MongoDB.Driver;
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
        [OptionalValidation(typeof(PasswordAttribute))]
        public Utils.Optional<string> OldPassword { get; set; }

        /// <summary>
        /// 获取新密码。
        /// </summary>
        [JsonProperty("password")]
        [Password]
        public string NewPassword { get; set; }

        /// <summary>
        /// 获取从当前数据模型创建的数据源更新定义。
        /// </summary>
        /// <returns>从当前数据模型创建的数据源更新定义。</returns>
        public UpdateDefinition<User> ToUpdateDefinition()
        {
            return Builders<User>.Update.Set(u => u.PasswordHash, PasswordUtils.GetPasswordHash(NewPassword));
        }
    }
}
