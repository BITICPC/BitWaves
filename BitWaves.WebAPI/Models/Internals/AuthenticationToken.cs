using System;
using BitWaves.Data.Entities;
using BitWaves.WebAPI.Utils;
using MongoDB.Bson;
using Newtonsoft.Json;

namespace BitWaves.WebAPI.Models.Internals
{
    /// <summary>
    /// 封装用户身份验证标识。
    /// </summary>
    public sealed class AuthenticationToken
    {
        /// <summary>
        /// 初始化 <see cref="AuthenticationToken"/> 类的新实例。
        /// </summary>
        [JsonConstructor]
        private AuthenticationToken()
        {
            CreationTime = DateTime.UtcNow;
        }

        /// <summary>
        /// 初始化包含给定的用户信息的 <see cref="AuthenticationToken"/> 对象。
        /// </summary>
        /// <param name="user">用户。</param>
        /// <exception cref="ArgumentNullException"><paramref name="user"/> 为 null。</exception>
        public AuthenticationToken(User user)
        {
            Contract.NotNull(user, nameof(user));

            UserIdRaw = user.Id.ToByteArray();
            IsAdmin = user.IsAdmin;
            CreationTime = DateTime.UtcNow;
        }

        /// <summary>
        /// 获取用户 ID 的二进制数据。
        /// </summary>
        [JsonProperty("userId")]
        public byte[] UserIdRaw { get; private set; }

        /// <summary>
        /// 获取用户 ID 的 <see cref="ObjectId"/> 表示。
        /// </summary>
        [JsonIgnore]
        public ObjectId UserId => new ObjectId(UserIdRaw);

        /// <summary>
        /// 获取用户是否为管理员。
        /// </summary>
        [JsonProperty("isAdmin")]
        public bool IsAdmin { get; private set; }

        /// <summary>
        /// 获取当前身份验证标识的创建时间。
        /// </summary>
        [JsonProperty("creationTime")]
        public DateTime CreationTime { get; private set; }
    }
}
