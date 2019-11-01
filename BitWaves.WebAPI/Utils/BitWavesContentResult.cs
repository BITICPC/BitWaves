using System;
using System.Threading.Tasks;
using BitWaves.Data.Entities;
using Microsoft.AspNetCore.Mvc;

namespace BitWaves.WebAPI.Utils
{
    /// <summary>
    /// 封装返回 BitWaves 静态内容的 <see cref="IActionResult"/> 实现。
    /// </summary>
    public sealed class BitWavesContentResult : IActionResult
    {
        /// <summary>
        /// 初始化 <see cref="BitWavesContentResult"/> 类的新实例。
        /// </summary>
        /// <param name="content">要返回的静态对象。</param>
        /// <exception cref="ArgumentNullException"><paramref name="content"/> 为 null。</exception>
        public BitWavesContentResult(Content content)
        {
            Contract.NotNull(content, nameof(content));

            Content = content;
            IsAttachment = false;
        }

        /// <summary>
        /// 获取要返回的静态内容。
        /// </summary>
        public Content Content { get; }

        /// <summary>
        /// 获取或设置静态内容返回时是否应该设置 Content-Disposition 响应头部为 attachment。
        /// </summary>
        public bool IsAttachment { get; set; }

        /// <inheritdoc />
        public async Task ExecuteResultAsync(ActionContext context)
        {
            if (IsAttachment)
            {
                var disposition = string.IsNullOrEmpty(Content.Name)
                    ? "attachment"
                    : $"attachment; filename=\"{Content.Name}\"";
                context.HttpContext.Response.Headers.Add("Content-Disposition", disposition);
            }

            context.HttpContext.Response.Headers.Add("Content-Type", Content.MimeType);
            await context.HttpContext.Response.Body.WriteAsync(Content.Data, 0, Content.Data.Length);
        }
    }
}
