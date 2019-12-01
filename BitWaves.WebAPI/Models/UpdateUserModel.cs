using System.ComponentModel.DataAnnotations;
using BitWaves.Data.Utils;
using BitWaves.WebAPI.Validation;
using Newtonsoft.Json;

using UrlAttribute = BitWaves.WebAPI.Validation.UrlAttribute;

namespace BitWaves.WebAPI.Models
{
    /// <summary>
    /// 为更新用户信息提供数据模型。
    /// </summary>
    public sealed class UpdateUserModel
    {
        /// <summary>
        /// 获取用户昵称。
        /// </summary>
        [JsonProperty("nickname")]
        [Inner(typeof(NicknameAttribute))]
        public Maybe<string> Nickname { get; private set; }

        /// <summary>
        /// 获取手机号。
        /// </summary>
        [JsonProperty("phone")]
        [Inner(typeof(PhoneNumberAttribute))]
        public Maybe<string> Phone { get; private set; }

        /// <summary>
        /// 获取电子邮箱地址。
        /// </summary>
        [JsonProperty("email")]
        [Inner(typeof(EmailAddressAttribute))]
        public Maybe<string> Email { get; private set; }

        /// <summary>
        /// 获取学校名称。
        /// </summary>
        [JsonProperty("school")]
        [Inner(typeof(SchoolAttribute))]
        public Maybe<string> School { get; private set; }

        /// <summary>
        /// 获取学号。
        /// </summary>
        [JsonProperty("studentId")]
        [Inner(typeof(StudentIdAttribute))]
        public Maybe<string> StudentId { get; private set; }

        /// <summary>
        /// 获取用户的博客 URL。
        /// </summary>
        [JsonProperty("blogUrl")]
        [Inner(typeof(UrlAttribute))]
        public Maybe<string> BlogUrl { get; private set; }
    }
}
