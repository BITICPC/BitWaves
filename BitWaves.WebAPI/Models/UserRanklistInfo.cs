using System;
using BitWaves.Data.Entities;
using Newtonsoft.Json;

namespace BitWaves.WebAPI.Models
{
    /// <summary>
    /// 为用户排名列表中的数据项目提供模型。
    /// </summary>
    public sealed class UserRanklistInfo
    {
        /// <summary>
        /// 初始化 <see cref="UserRanklistInfo"/> 的新实例。
        /// </summary>
        /// <param name="entity">用户实体对象。</param>
        /// <exception cref="ArgumentNullException"><paramref name="entity"/> 为 null。</exception>
        public UserRanklistInfo(User entity)
        {
            Contract.NotNull(entity, nameof(entity));

            Username = entity.Username;
            JoinTime = entity.JoinTime;
            TotalSubmissions = entity.TotalSubmissions;
            TotalAccepted = entity.TotalAcceptedSubmissions;
            TotalProblemsAttempted = entity.TotalProblemsAttempted;
            TotalProblemsAccepted = entity.TotalProblemsAccepted;
        }

        /// <summary>
        /// 获取用户名。
        /// </summary>
        [JsonProperty("username")]
        public string Username { get; }

        /// <summary>
        /// 获取用户的加入时间，UTC 时区。
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
        public int TotalAccepted { get; }

        /// <summary>
        /// 获取用户总共尝试的题目数量。
        /// </summary>
        [JsonProperty("totalProblemsAttempted")]
        public int TotalProblemsAttempted { get; }

        /// <summary>
        /// 获取用户总共 AC 的题目数量。
        /// </summary>
        [JsonProperty("totalProblemsAccepted")]
        public int TotalProblemsAccepted { get; }
    }
}
