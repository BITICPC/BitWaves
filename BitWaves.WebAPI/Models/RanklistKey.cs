using System;
using System.Linq.Expressions;
using BitWaves.Data.Entities;

namespace BitWaves.WebAPI.Models
{
    /// <summary>
    /// 表示用户排名列表的排名依据。
    /// </summary>
    public enum RanklistKey
    {
        /// <summary>
        /// 按照用户总提交数量进行排名。
        /// </summary>
        TotalSubmissions,

        /// <summary>
        /// 按照用户总 AC 提交数量进行排名。
        /// </summary>
        TotalAccepted,

        /// <summary>
        /// 按照用户总共尝试的题目数量进行排名。
        /// </summary>
        TotalProblemsAttempted,

        /// <summary>
        /// 按照用户通过的题目数量进行排名。
        /// </summary>
        TotalProblemsAccepted
    }

    /// <summary>
    /// 为 <see cref="RanklistKey"/> 提供扩展方法。
    /// </summary>
    public static class RanklistKeyExtensions
    {
        /// <summary>
        /// 获取与给定的 <see cref="RanklistKey"/> 相对应的成员选择器。
        /// </summary>
        /// <param name="key">排序键。</param>
        /// <returns>与给定的排序键相对应的成员选择器。</returns>w
        public static Expression<Func<User, object>> GetKeySelector(this RanklistKey key)
        {
            switch (key)
            {
                case RanklistKey.TotalAccepted:
                    return u => u.TotalAcceptedSubmissions;
                case RanklistKey.TotalSubmissions:
                    return u => u.TotalSubmissions;
                case RanklistKey.TotalProblemsAccepted:
                    return u => u.TotalProblemsAccepted;
                case RanklistKey.TotalProblemsAttempted:
                    return u => u.TotalProblemsAttempted;
                default:
                    throw new Exception("Unreachable code.");
            }
        }
    }
}
