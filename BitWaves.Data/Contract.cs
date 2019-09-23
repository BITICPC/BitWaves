using System;

namespace BitWaves.Data
{
    /// <summary>
    /// 提供常用的前置约束检查。
    /// </summary>
    internal static class Contract
    {
        /// <summary>
        /// 检查给定的引用类型的值不为 null，否则抛出 <see cref="ArgumentNullException"/>。
        /// </summary>
        /// <param name="value">要检查的值。</param>
        /// <param name="variableName">变量名称。</param>
        /// <typeparam name="T">要检查的值的类型。必须为引用类型。</typeparam>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> 为 null。</exception>
        public static void NotNull<T>(T value, string variableName) where T : class
        {
            if (value == null)
                throw new ArgumentNullException(variableName);
        }

        /// <summary>
        /// 检查给定的字符串不为空串。
        /// </summary>
        /// <param name="value">要检查的字符串。</param>
        /// <param name="message"><see cref="ArgumentException"/> 的异常信息。</param>
        /// <param name="variableName">变量名称。</param>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> 为 null。</exception>
        /// <exception cref="ArgumentException"><paramref name="value"/> 为空串。</exception>
        public static void NotNullOrEmpty(string value, string message, string variableName)
        {
            NotNull(value, variableName);

            if (value.Length == 0)
                throw new ArgumentException(message, variableName);
        }
    }
}
