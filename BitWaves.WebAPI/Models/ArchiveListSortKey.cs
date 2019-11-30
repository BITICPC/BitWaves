using System;
using System.Linq.Expressions;
using BitWaves.Data.Entities;

namespace BitWaves.WebAPI.Models
{
    /// <summary>
    /// 为 Problem Archive 的题目列表排序提供排序关键字。
    /// </summary>
    public enum ArchiveListSortKey
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
        AcceptedSubmissions,

        /// <summary>
        /// 按照尝试过题目的用户数量进行排序。
        /// </summary>
        TotalAttemptedUsers,

        /// <summary>
        /// 按照通过题目的用户数量进行排序。
        /// </summary>
        TotalSolvedUsers
    }

    /// <summary>
    /// 为 <see cref="ArchiveListSortKey"/> 提供扩展方法。
    /// </summary>
    public static class ArchiveListKeyExtensions
    {
        /// <summary>
        /// 从给定的 <see cref="ArchiveListSortKey"/> 获取成员选择器。
        /// </summary>
        /// <param name="key"><see cref="ArchiveListSortKey"/> 值。</param>
        /// <returns>与给定的 <see cref="ArchiveListSortKey"/> 值对应的成员选择器。</returns>
        public static Expression<Func<Problem, object>> GetFieldSelector(this ArchiveListSortKey key)
        {
            switch (key)
            {
                case ArchiveListSortKey.Id:
                    return problem => problem.ArchiveId;
                case ArchiveListSortKey.Difficulty:
                    return problem => problem.Difficulty;
                case ArchiveListSortKey.LastUpdateTime:
                    return problem => problem.LastUpdateTime;
                case ArchiveListSortKey.TotalSubmissions:
                    return problem => problem.TotalSubmissions;
                case ArchiveListSortKey.AcceptedSubmissions:
                    return problem => problem.AcceptedSubmissions;
                case ArchiveListSortKey.TotalAttemptedUsers:
                    return problem => problem.TotalAttemptedUsers;
                case ArchiveListSortKey.TotalSolvedUsers:
                    return problem => problem.TotalSolvedUsers;
                default:
                    throw new Exception("Unreachable code.");
            }
        }
    }
}