using System;

namespace BitWaves.WebAPI.Utils
{
    /// <summary>
    /// 提供前置约束检查逻辑。
    /// </summary>
    internal static class Contract
    {
        /// <summary>
        /// 检查给定的引用类型的值不为 null。
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
        /// 检查给定的有符号值不为负值。
        /// </summary>
        /// <param name="value">要检查的值。</param>
        /// <param name="variableName">变量名称。</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="value"/> 为负。</exception>
        public static void NonNegative(long value, string variableName)
        {
            if (value < 0)
                throw new ArgumentOutOfRangeException(variableName);
        }

        /// <summary>
        /// 检查给定的有符号值为正值。
        /// </summary>
        /// <param name="value">要检查的值。</param>
        /// <param name="variableName">变量名称。</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="value"/> 为负或零。</exception>
        public static void Positive(long value, string variableName)
        {
            if (value <= 0)
                throw new ArgumentOutOfRangeException(variableName);
        }
    }
}
