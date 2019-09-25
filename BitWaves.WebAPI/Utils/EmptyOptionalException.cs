using System;
using System.Runtime.Serialization;

namespace BitWaves.WebAPI.Utils
{
    /// <summary>
    /// 当尝试访问一个空的 <see cref="Optional{T}"/> 中的值时抛出。
    /// </summary>
    [Serializable]
    public sealed class EmptyOptionalException : Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        /// <summary>
        /// 初始化 <see cref="EmptyOptionalException"/> 的新实例。
        /// </summary>
        public EmptyOptionalException()
            : this("尝试访问空的 Optional 实例。")
        {
        }

        /// <summary>
        /// 初始化 <see cref="EmptyOptionalException"/> 的新实例。
        /// </summary>
        /// <param name="message">异常消息。</param>
        public EmptyOptionalException(string message) : base(message)
        {
        }

        /// <summary>
        /// 从给定的序列化环境中反序列化 <see cref="EmptyOptionalException"/> 的新实例。
        /// </summary>
        /// <param name="info">序列化信息。</param>
        /// <param name="context">序列化环境的流上下文。</param>
        private EmptyOptionalException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}
