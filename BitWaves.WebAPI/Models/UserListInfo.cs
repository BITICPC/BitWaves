using System;
using Newtonsoft.Json;

namespace BitWaves.WebAPI.Models
{
    public class UserListInfo
    {
        /// <summary>
        /// 获取或设置用户名。
        /// </summary>
        [JsonProperty("username")]
        public string Username { get; set; }

        /// <summary>
        /// 获取或设置用户的昵称。
        /// </summary>
        [JsonProperty("nickname")]
        public string Nickname { get; set; }

        /// <summary>
        /// 获取或设置用户的加入时间。
        /// </summary>
        [JsonProperty("joinTime")]
        public DateTime JoinTime { get; set; }

        /// <summary>
        /// 获取或设置用户的总提交数。
        /// </summary>
        [JsonProperty("totalSubmissions")]
        public int TotalSubmissions { get; set; }

        /// <summary>
        /// 获取或设置用户的总 AC 提交数。
        /// </summary>
        [JsonProperty("totalAccepted")]
        public int TotalAcceptedSubmissions { get; set; }

        /// <summary>
        /// 获取或设置用户的总提交题目数。
        /// </summary>
        [JsonProperty("totalProblemsAttempted")]
        public int TotalProblemsAttempted { get; set; }

        /// <summary>
        /// 获取或设置用户的 AC 题目数。
        /// </summary>
        [JsonProperty("totalProblemsAccepted")]
        public int TotalProblemsAccepted { get; set; }
    }
}
