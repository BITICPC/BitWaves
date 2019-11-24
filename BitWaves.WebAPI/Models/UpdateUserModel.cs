using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BitWaves.Data.Entities;
using BitWaves.WebAPI.Validation;
using MongoDB.Driver;
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
        [OptionalValidation(typeof(NicknameAttribute))]
        public Utils.Optional<string> Nickname { get; private set; }

        /// <summary>
        /// 获取手机号。
        /// </summary>
        [JsonProperty("phone")]
        [OptionalValidation(typeof(PhoneNumberAttribute))]
        public Utils.Optional<string> Phone { get; private set; }

        /// <summary>
        /// 获取电子邮箱地址。
        /// </summary>
        [JsonProperty("email")]
        [OptionalValidation(typeof(EmailAddressAttribute))]
        public Utils.Optional<string> Email { get; private set; }

        /// <summary>
        /// 获取学校名称。
        /// </summary>
        [JsonProperty("school")]
        [OptionalValidation(typeof(SchoolAttribute))]
        public Utils.Optional<string> School { get; private set; }

        /// <summary>
        /// 获取学号。
        /// </summary>
        [JsonProperty("studentId")]
        [OptionalValidation(typeof(StudentIdAttribute))]
        public Utils.Optional<string> StudentId { get; private set; }

        /// <summary>
        /// 获取用户的博客 URL。
        /// </summary>
        [JsonProperty("blogUrl")]
        [OptionalValidation(typeof(UrlAttribute))]
        public Utils.Optional<string> BlogUrl { get; private set; }

        /// <summary>
        /// 从当前的数据模型创建 <see cref="UpdateDefinition{User}"/> 对象以更新数据库。
        /// </summary>
        /// <returns>创建的 <see cref="UpdateDefinition{User}"/> 对象。若没有任何数据需要更新，返回 null。</returns>
        public UpdateDefinition<User> ToUpdateDefinition()
        {
            var updates = new List<UpdateDefinition<User>>();

            if (Nickname.HasValue)
            {
                updates.Add(Builders<User>.Update.Set(u => u.Nickname, Nickname.Value));
            }

            if (Phone.HasValue)
            {
                updates.Add(Builders<User>.Update.Set(u => u.Phone, Phone.Value));
            }

            if (Email.HasValue)
            {
                updates.Add(Builders<User>.Update.Set(u => u.Email, Email.Value));
            }

            if (School.HasValue)
            {
                updates.Add(Builders<User>.Update.Set(u => u.School, School.Value));
            }

            if (StudentId.HasValue)
            {
                updates.Add(Builders<User>.Update.Set(u => u.StudentId, StudentId.Value));
            }

            if (BlogUrl.HasValue)
            {
                updates.Add(Builders<User>.Update.Set(u => u.BlogUrl, BlogUrl.Value));
            }

            return updates.Count > 0
                ? Builders<User>.Update.Combine(updates)
                : null;
        }
    }
}
