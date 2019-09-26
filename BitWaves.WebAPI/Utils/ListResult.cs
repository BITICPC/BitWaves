using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

namespace BitWaves.WebAPI.Utils
{
    /// <summary>
    /// 提供 BitWaves 分页列表结果的 <see cref="IActionResult"/> 实现。
    /// </summary>
    /// <typeparam name="T">列表中元素的类型。</typeparam>
    public sealed class ListResult<T> : ObjectResult
    {
        private readonly long _count;

        /// <summary>
        /// 初始化 <see cref="ListResult{T}"/> 的新实例。
        /// </summary>
        /// <param name="count">分页前列表中的元素总数。</param>
        /// <param name="viewList">当前页内的元素列表。</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="count"/> 为负。</exception>
        public ListResult(long count, IEnumerable<T> viewList)
            : base(viewList)
        {
            Contract.NonNegative(count, nameof(count));
            _count = count;
        }

        /// <summary>
        /// 向响应头部中加入 `X-BitWaves-Count` 字段。
        /// </summary>
        /// <param name="context">上下文。</param>
        private void AddHeader(ActionContext context)
        {
            context.HttpContext.Response.Headers.Add("X-BitWaves-Count", new StringValues(_count.ToString()));
        }

        /// <inheritdoc />
        public override async Task ExecuteResultAsync(ActionContext context)
        {
            AddHeader(context);
            await base.ExecuteResultAsync(context);
        }

        /// <inheritdoc />
        public override void ExecuteResult(ActionContext context)
        {
            AddHeader(context);
            base.ExecuteResult(context);
        }
    }
}
