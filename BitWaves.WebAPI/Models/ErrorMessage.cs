using Newtonsoft.Json;

namespace BitWaves.WebAPI.Models
{
    /// <summary>
    /// 封装应用程序错误消息。
    /// </summary>
    public sealed class ErrorMessage
    {
        /// <summary>
        /// 初始化 <see cref="ErrorMessage"/> 类的新实例。
        /// </summary>
        /// <param name="errorCode">错误代码。</param>
        /// <param name="message">错误消息。</param>
        public ErrorMessage(int errorCode, string message)
        {
            ErrorCode = errorCode;
            Message = message;
        }

        /// <summary>
        /// 获取错误代码。
        /// </summary>
        [JsonProperty("errorCode")]
        public int ErrorCode { get; }

        /// <summary>
        /// 获取错误消息。
        /// </summary>
        [JsonProperty("message")]
        public string Message { get; }
    }
}
