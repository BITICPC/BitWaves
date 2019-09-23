using System;
using BitWaves.Data.Entities;
using BitWaves.WebAPI.Utils;
using Newtonsoft.Json;

namespace BitWaves.WebAPI.Models
{
    /// <summary>
    /// 封装登录结果信息。
    /// </summary>
    public sealed class LoginResult
    {
        /// <summary>
        /// 初始化 <see cref="LoginResult"/> 的新实例。
        /// </summary>
        /// <param name="user">用户信息。</param>
        /// <param name="jwt">编码用户身份验证标识的 JWT。</param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="user"/> 为 null
        ///     或
        ///     <paramref name="jwt"/> 为 null。
        /// </exception>
        public LoginResult(User user, string jwt)
        {
            Contract.NotNull(user, nameof(user));
            Contract.NotNull(jwt, nameof(jwt));

            Username = user.Username;
            IsAdmin = user.IsAdmin;
            Jwt = jwt;
        }

        /// <summary>
        /// 获取用户名。
        /// </summary>
        [JsonProperty("username")]
        public string Username { get; }

        /// <summary>
        /// 获取用户是否为管理员。
        /// </summary>
        [JsonProperty("isAdmin")]
        public bool IsAdmin { get; }

        /// <summary>
        /// 获取编码用户身份标识的 JWT。
        /// </summary>
        [JsonProperty("jwt")]
        public string Jwt { get; }
    }
}
