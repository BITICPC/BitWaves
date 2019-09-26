using System;
using System.Linq.Expressions;
using BitWaves.Data.Entities;

namespace BitWaves.WebAPI.Models
{
    /// <summary>
    /// 为 Problem Archive 的题目列表排序提供排序关键字。
    /// </summary>
    public enum ArchiveListKey
    {
        /// <summary>
        /// 按照题目的 ArchiveId 进行排序。
        /// </summary>
        Id,

        /// <summary>
        /// 按照题目的难度进行排序。
        /// </summary>
        Difficulty,

        /// <summary>
        /// 按照题目的上次修改时间进行排序。
        /// </summary>
        LastUpdateTime,

        /// <summary>
        /// 按照题目的总提交数量进行排序。
        /// </summary>
        TotalSubmissions,

        /// <summary>
        /// 按照题目的 AC 提交数量进行排序。
        /// </summary>
        AcceptedSubmissions
    }

    /// <summary>
    /// 为 <see cref="ArchiveListKey"/> 提供扩展方法。
    /// </summary>
    public static class ArchiveListKeyExtensions
    {
        /// <summary>
        /// 从给定的 <see cref="ArchiveListKey"/> 获取成员选择器。
        /// </summary>
        /// <param name="key"><see cref="ArchiveListKey"/> 值。</param>
        /// <returns>与给定的 <see cref="ArchiveListKey"/> 值对应的成员选择器。</returns>
        public static Expression<Func<Problem, object>> GetFieldSelector(this ArchiveListKey key)
        {
            switch (key)
            {
                case ArchiveListKey.Id:
                    return problem => problem.ArchiveId;
                case ArchiveListKey.Difficulty:
                    return problem => problem.Difficulty;
                case ArchiveListKey.LastUpdateTime:
                    return problem => problem.LastUpdateTime;
                case ArchiveListKey.TotalSubmissions:
                    return problem => problem.TotalSubmissions;
                case ArchiveListKey.AcceptedSubmissions:
                    return problem => problem.AcceptedSubmissions;
                default:
                    throw new Exception("Unreachable code.");
            }
        }
    }
}
