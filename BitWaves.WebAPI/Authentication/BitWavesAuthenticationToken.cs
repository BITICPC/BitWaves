using System;
using System.Collections.Generic;
using System.Security.Claims;
using BitWaves.Data.Entities;
using BitWaves.WebAPI.Utils;
using Microsoft.AspNetCore.Authentication;
using MongoDB.Bson;
using Newtonsoft.Json;

namespace BitWaves.WebAPI.Authentication
{
    /// <summary>
    /// 封装用户身份验证标识。
    /// </summary>
    public sealed class BitWavesAuthenticationToken
    {
        /// <summary>
        /// 初始化 <see cref="BitWavesAuthenticationToken"/> 类的新实例。
        /// </summary>
        [JsonConstructor]
        private BitWavesAuthenticationToken()
        {
            CreationTime = DateTime.UtcNow;
        }

        /// <summary>
        /// 初始化包含给定的用户信息的 <see cref="BitWavesAuthenticationToken"/> 对象。
        /// </summary>
        /// <param name="user">用户。</param>
        /// <exception cref="ArgumentNullException"><paramref name="user"/> 为 null。</exception>
        public BitWavesAuthenticationToken(User user)
        {
            Contract.NotNull(user, nameof(user));

            UserIdRaw = user.Id.ToByteArray();
            Username = user.Username;
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
        /// 获取用户名。
        /// </summary>
        [JsonProperty("username")]
        public string Username { get; private set; }

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

        /// <summary>
        /// 从当前的身份验证标识创建 <see cref="AuthenticationTicket"/> 对象。
        /// </summary>
        /// <returns>创建的 <see cref="AuthenticationTicket"/> 对象。</returns>
        public AuthenticationTicket GetAuthenticationTicket()
        {
            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Name, Username));
            claims.Add(new Claim(ClaimTypes.Role,
                                 IsAdmin ? BitWavesAuthDefaults.AdminRoleName : BitWavesAuthDefaults.NonAdminRoleName));
            claims.Add(new Claim(BitWavesAuthDefaults.IdentityCreationTime, CreationTime.ToLongTimeString()));

            var principle = new ClaimsPrincipal(new ClaimsIdentity(claims, BitWavesAuthDefaults.AuthenticateType));
            return new AuthenticationTicket(principle, BitWavesAuthDefaults.SchemeName);
        }
    }
}
