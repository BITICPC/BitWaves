using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace BitWaves.WebAPI.Utils
{
    /// <summary>
    /// 包装一个 <see cref="PaginatedListResult{T}"/> 对象或者一个 <see cref="ActionResult"/> 对象。
    /// </summary>
    public sealed class PaginatedListActionResult<TElement> : IConvertToActionResult
    {
        private readonly ActionResult _result;

        /// <summary>
        /// 初始化 <see cref="PaginatedListActionResult{TElement}"/> 类的新实例。
        /// </summary>
        /// <param name="result">要包装的 <see cref="ActionResult"/> 对象。</param>
        /// <exception cref="ArgumentNullException"><paramref name="result"/> 为 null。</exception>
        public PaginatedListActionResult(ActionResult result)
        {
            Contract.NotNull(result, nameof(result));

            _result = result;
        }

        /// <summary>
        /// 初始化 <see cref="PaginatedListActionResult{TElement}"/> 类的新实例。
        /// </summary>
        /// <param name="totalCount">未分页前符合筛选条件的元素总数量。</param>
        /// <param name="list">分页后的数据元素列表。</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="totalCount"/> 为负。</exception>
        /// <exception cref="ArgumentNullException"><paramref name="list"/> 为 null。</exception>
        public PaginatedListActionResult(long totalCount, IEnumerable<TElement> list)
        {
            Contract.NonNegative(totalCount, nameof(totalCount));
            Contract.NotNull(list, nameof(list));

            _result = new PaginatedListResult<TElement>(totalCount, list);
        }

        /// <inheritdoc />
        public IActionResult Convert()
        {
            return _result;
        }

        /// <summary>
        /// 将给定的 <see cref="ActionResult"/> 对象转换为 <see cref="PaginatedListActionResult{TElement}"/> 包装。
        /// </summary>
        /// <param name="result">要包装的 <see cref="ActionResult"/> 对象。</param>
        /// <returns>创建的 <see cref="PaginatedListActionResult{TElement}"/> 包装。</returns>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="result"/> 为 null。
        /// </exception>
        public static implicit operator PaginatedListActionResult<TElement>(ActionResult result)
        {
            Contract.NotNull(result, nameof(result));

            return new PaginatedListActionResult<TElement>(result);
        }

        /// <summary>
        /// 将给定的 <see cref="PaginatedListResult{T}"/> 二元组转换为 <see cref="PaginatedListActionResult{TElement}"/>
        /// 包装。
        /// </summary>
        /// <param name="list">
        /// 包含要返回的列表数据的二元组。该二元组的第一个成员表示分页前符合筛选条件的数据条目的总数量，第二个成员表示分页后的列表。
        /// </param>
        /// <returns>创建的 <see cref="PaginatedListActionResult{TElement}"/> 包装。</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="list"/> 的第一个成员为负。</exception>
        /// <exception cref="ArgumentNullException"><paramref name="list"/> 的第二个成员为 null。</exception>
        public static implicit operator PaginatedListActionResult<TElement>((long, IEnumerable<TElement>) list)
        {
            Contract.NonNegative(list.Item1, nameof(list));
            Contract.NotNull(list.Item2, nameof(list));

            return new PaginatedListActionResult<TElement>(list.Item1, list.Item2);
        }
    }
}
