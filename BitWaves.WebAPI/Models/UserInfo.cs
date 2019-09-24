using System;
using BitWaves.Data.Entities;
using BitWaves.WebAPI.Utils;
using Newtonsoft.Json;

namespace BitWaves.WebAPI.Models
{
    /// <summary>
    /// 提供用户信息数据模型。
    /// </summary>
    public sealed class UserInfo
    {
        /// <summary>
        /// 初始化 <see cref="UserInfo"/> 类的新实例。
        /// </summary>
        /// <param name="entity">用户实体对象。</param>
        /// <exception cref="ArgumentNullException"><paramref name="entity"/> 为 null。</exception>
        public UserInfo(User entity)
        {
            Contract.NotNull(entity, nameof(entity));

            Username = entity.Username;
            JoinTime = entity.JoinTime;
            TotalSubmissions = entity.TotalSubmissions;
            TotalAcceptedSubmissions = entity.TotalAcceptedSubmissions;
            TotalProblemsAttempted = entity.TotalProblemsAttempted;
            TotalProblemsAccepted = entity.TotalProblemsAccepted;
        }

        /// <summary>
        /// 获取用户名。
        /// </summary>
        [JsonProperty("username")]
        public string Username { get; }

        /// <summary>
        /// 获取用户的加入时间。
        /// </summary>
        [JsonProperty("joinTime")]
        public DateTime JoinTime { get; }

        /// <summary>
        /// 获取用户的总提交数。
        /// </summary>
        [JsonProperty("totalSubmissions")]
        public int TotalSubmissions { get; }

        /// <summary>
        /// 获取用户的总 AC 提交数。
        /// </summary>
        [JsonProperty("totalAccepted")]
        public int TotalAcceptedSubmissions { get; }

        /// <summary>
        /// 获取用户的总提交题目数。
        /// </summary>
        [JsonProperty("totalProblemsAttempted")]
        public int TotalProblemsAttempted { get; }

        /// <summary>
        /// 获取用户的 AC 题目数。
        /// </summary>
        [JsonProperty("totalProblemsAccepted")]
        public int TotalProblemsAccepted { get; }
    }
}
