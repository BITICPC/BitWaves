using System;
using MongoDB.Driver;

namespace BitWaves.WebAPI.Extensions
{
    /// <summary>
    /// 为 <see cref="IFindFluent{TDocument, TProjection}"/> 提供扩展方法。
    /// </summary>
    internal static class FindFluentExtensions
    {
        /// <summary>
        /// 在给定的 <see cref="IFindFluent{TDocument, TProjection}"/> 实例上执行分页操作。
        /// </summary>
        /// <param name="source">源 <see cref="IFindFluent{TDocument, TProjection}"/> 对象。</param>
        /// <param name="page">页码，从 0 开始编号。</param>
        /// <param name="itemsPerPage">每一页上的元素数量。</param>
        /// <typeparam name="TDocument"></typeparam>
        /// <typeparam name="TProjection"></typeparam>
        /// <returns>执行分页后的 <see cref="IFindFluent{TDocument, TProjection}"/> 对象。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> 为 null。</exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="page"/> 为负
        ///     或
        ///     <paramref name="itemsPerPage"/> 不为正。
        /// </exception>
        /// <exception cref="OverflowException">
        ///     <paramref name="page"/> 与 <paramref name="itemsPerPage"/> 的乘积发生了 32 位整数溢出。
        /// </exception>
        public static IFindFluent<TDocument, TProjection> Paginate<TDocument, TProjection>(
            this IFindFluent<TDocument, TProjection> source, int page, int itemsPerPage)
        {
            Contract.NotNull(source, nameof(source));
            Contract.NonNegative(page, nameof(page));
            Contract.Positive(itemsPerPage, nameof(itemsPerPage));

            var skip = checked(page * itemsPerPage);
            return source.Skip(skip).Limit(itemsPerPage);
        }
    }
}
