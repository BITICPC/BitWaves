using Newtonsoft.Json;

namespace BitWaves.WebAPI.Models
{
    /// <summary>
    /// 提供用户信息数据模型。
    /// </summary>
    public class UserInfo : UserListInfo
    {
        /// <summary>
        /// 获取或设置用户的手机号。
        /// </summary>
        [JsonProperty("phone")]
        public string Phone { get; set; }

        /// <summary>
        /// 获取或设置用户的电子邮箱。
        /// </summary>
        [JsonProperty("email")]
        public string Email { get; set; }

        /// <summary>
        /// 获取或设置用户的学校。
        /// </summary>
        [JsonProperty("school")]
        public string School { get; set; }

        /// <summary>
        /// 获取或设置用户的学号。
        /// </summary>
        [JsonProperty("studentId")]
        public string StudentId { get; set; }

        /// <summary>
        /// 获取或设置用户的博客地址。
        /// </summary>
        [JsonProperty("blogUrl")]
        public string BlogUrl { get; set; }

        /// <summary>
        /// 获取或设置用户的排名。
        /// </summary>
        [JsonProperty("rank")]
        public long Rank { get; set; }

        /// <summary>
        /// 获取或设置用户是否为管理员。
        /// </summary>
        [JsonProperty("isAdmin")]
        public bool IsAdmin { get; set; }
    }
}
