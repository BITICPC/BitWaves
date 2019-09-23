using System.ComponentModel.DataAnnotations;
using BitWaves.Data.Entities;
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
        [Required(ErrorMessage = "用户名未填写")]
        [MinLength(3, ErrorMessage = "用户名长度至少为3个字符")]
        public string Username { get; private set; }

        /// <summary>
        /// 获取用户密码。
        /// </summary>
        [JsonProperty("password")]
        [Required(ErrorMessage = "密码未填写")]
        [MinLength(6, ErrorMessage = "密码长度至少为6个字符")]
        public string Password { get; private set; }

        /// <summary>
        /// 获取用户手机号。
        /// </summary>
        [JsonProperty("phone")]
        [Required(ErrorMessage = "手机号未填写")]
        [StringLength(11, ErrorMessage = "手机号长度应为11个字符")]
        [RegularExpression(@"^\d{11}", ErrorMessage = "手机号应由11个数字组成")]
        public string Phone { get; private set; }

        /// <summary>
        /// 从当前数据模型对象创建 <see cref="User"/> 实体对象。
        /// </summary>
        /// <returns>创建的 <see cref="User"/> 实体对象。</returns>
        public User ToUserEntity()
        {
            var entity = User.Create();
            entity.Username = Username;
            entity.SetPassword(Password);
            entity.Phone = Phone;
            return entity;
        }
    }
}
