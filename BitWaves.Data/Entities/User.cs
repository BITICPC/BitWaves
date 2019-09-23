using System;
using System.Security.Cryptography;
using System.Text;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BitWaves.Data.Entities
{
    /// <summary>
    /// 表示一个用户。
    /// </summary>
    public sealed class User
    {
        /// <summary>
        /// 获取或设置用户 ID。
        /// </summary>
        [BsonId]
        public ObjectId Id { get; private set; }

        /// <summary>
        /// 获取用户名。
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// 获取用户密码的 SHA256 哈希值。
        /// </summary>
        public byte[] PasswordHash { get; set; }

        /// <summary>
        /// 获取用户手机号。
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// 获取或设置用户学号。
        /// </summary>
        public string StudentId { get; set; }

        /// <summary>
        /// 获取用户的注册时间。
        /// </summary>
        public DateTime JoinTime { get; set; }

        /// <summary>
        /// 获取用户的总提交数。
        /// </summary>
        public int TotalSubmissions { get; set; }

        /// <summary>
        /// 获取用户的总 AC 提交数。
        /// </summary>
        public int TotalAcceptedSubmissions { get; set; }

        /// <summary>
        /// 获取用户曾经提交过的题目总数。
        /// </summary>
        public int TotalProblemsAttempted { get; set; }

        /// <summary>
        /// 获取用户通过的题目总数。
        /// </summary>
        public int TotalProblemsAccepted { get; set; }

        /// <summary>
        /// 获取用户管理员标志位。
        /// </summary>
        public bool IsAdmin { get; set; }

        /// <summary>
        /// 设置用户密码。
        /// </summary>
        /// <param name="password">用户密码明文。</param>
        /// <exception cref="ArgumentNullException"><paramref name="password"/> 为 null。</exception>
        public void SetPassword(string password)
        {
            PasswordHash = GetPasswordHash(password);
        }

        /// <summary>
        /// 测试给定的密码的哈希值是否与保存的密码哈希值一致。
        /// </summary>
        /// <param name="password">要测试的密码。</param>
        /// <returns>给定密码的哈希值是否与保存的密码哈希值一致。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="password"/> 为 null。</exception>
        public bool Challenge(string password)
        {
            Contract.NotNull(password, nameof(password));

            return BufferUtils.Equals(PasswordHash, GetPasswordHash(password));
        }

        /// <summary>
        /// 创建一个新的 <see cref="User"/> 对象。
        /// </summary>
        /// <returns>新创建的 <see cref="User"/> 对象。</returns>
        public static User Create()
        {
            return new User
            {
                Id = ObjectId.GenerateNewId(),
                JoinTime = DateTime.UtcNow
            };
        }

        /// <summary>
        /// 获取给定密码的哈希值。
        /// </summary>
        /// <param name="password">要哈希的密码明文。</param>
        /// <returns>密码哈希值。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="password"/> 为 null。</exception>
        private static byte[] GetPasswordHash(string password)
        {
            Contract.NotNull(password, nameof(password));

            var encoded = Encoding.UTF8.GetBytes(password);
            using (var hasher = SHA256.Create())
            {
                return hasher.ComputeHash(encoded);
            }
        }
    }
}
