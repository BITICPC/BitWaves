using System.Net;
using BitWaves.WebAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace BitWaves.WebAPI.Extensions
{
    /// <summary>
    /// 为 <see cref="ControllerBase"/> 提供扩展方法。
    /// </summary>
    internal static class ControllerExtensions
    {
        /// <summary>
        /// 返回封装 BitWaves 错误消息的 <see cref="IActionResult"/> 对象。
        /// </summary>
        /// <param name="_"></param>
        /// <param name="errorCode">错误代码。</param>
        /// <param name="message">错误消息。</param>
        /// <returns>封装 BitWaves 错误消息的 <see cref="IActionResult"/> 对象。</returns>
        public static IActionResult ErrorMessage(this ControllerBase _, int errorCode, string message)
        {
            return new ObjectResult(new ErrorMessage(errorCode, message))
            {
                StatusCode = (int) HttpStatusCode.UnprocessableEntity
            };
        }
    }
}
