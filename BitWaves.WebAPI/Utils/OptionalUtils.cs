using System;

namespace BitWaves.WebAPI.Utils
{
    /// <summary>
    /// 为 <see cref="Optional{T}"/> 提供基于反射的访问。
    /// </summary>
    public static class OptionalUtils
    {
        /// <summary>
        /// 测试给定的类型是否为 <see cref="Optional{T}"/> 的某个实例化类型。
        /// </summary>
        /// <param name="type">要测试的类型。</param>
        /// <returns>给定的类型是否为 <see cref="Optional{T}"/> 的某个实例化类型。</returns>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="type"/> 为 null。
        /// </exception>
        public static bool IsOptionalType(Type type)
        {
            Contract.NotNull(type, nameof(type));

            return type.IsConstructedGenericType && type.GetGenericTypeDefinition() == typeof(Optional<>);
        }

        /// <summary>
        /// 前置约束保证 <paramref name="optional"/> 为 <see cref="Optional{T}"/> 的实例对象。
        /// </summary>
        /// <param name="optional">要测试的值。</param>
        /// <exception cref="ArgumentException">
        ///     <paramref name="optional"/> 不是 <see cref="Optional{T}"/> 的实例对象。
        /// </exception>
        private static void EnsureIsOptional(object optional)
        {
            if (!IsOptionalType(optional.GetType()))
                throw new ArgumentException("optional 不是 Optional<T> 的实例对象。");
        }

        /// <summary>
        /// 获取给定的 <see cref="Optional{T}"/> 实例中的值。
        /// </summary>
        /// <param name="optional">装箱后的 <see cref="Optional{T}"/> 实例对象。</param>
        /// <param name="value">输出参数，输出为给定的 <see cref="Optional{T}"/> 中的值。</param>
        /// <returns>给定的 <see cref="Optional{T}"/> 对象是否包含值。</returns>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="optional"/> 为 null。
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="optional"/> 不是 <see cref="Optional{T}"/> 的实例对象。
        /// </exception>
        public static bool TryGetOptionalValue(object optional, out object value)
        {
            Contract.NotNull(optional, nameof(optional));
            EnsureIsOptional(optional);

            var optionalType = optional.GetType();
            var getValueMethod = optionalType.GetMethod(nameof(Optional<object>.TryGetValue));
            if (getValueMethod == null)
            {
                throw new Exception("Unexpected error: failed to find `TryGetValue` method on Optional<T>.");
            }

            var invokeArgs = new object[1];
            if (!(bool) getValueMethod.Invoke(optional, invokeArgs))
            {
                value = null;
                return false;
            }

            value = invokeArgs[0];
            return true;
        }

        /// <summary>
        /// 设置给定的 <see cref="Optional{T}"/> 对象中的值。
        /// </summary>
        /// <param name="optional">装箱后的 <see cref="Optional{T}"/> 实例对象。</param>
        /// <param name="value">要设置的值。</param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="optional"/> 为 null。
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="value"/> 的类型与 <see cref="Optional{T}"/> 的值类型不匹配。
        /// </exception>
        public static void SetOptionalValue(object optional, object value)
        {
            Contract.NotNull(optional, nameof(optional));
            EnsureIsOptional(optional);

            var setValueMethod = optional.GetType().GetMethod(nameof(Optional<object>.Set));
            if (setValueMethod == null)
            {
                throw new Exception("Unexpected error: failed to find `Set` method on Optional<T>.");
            }

            setValueMethod.Invoke(optional, new[] { value });
        }

        /// <summary>
        /// 创建一个包含给定的值的 <see cref="Optional{T}"/> 对象。
        /// </summary>
        /// <param name="valueType">要包装的值的类型。</param>
        /// <param name="value">要包装的值。</param>
        /// <returns>包含给定值的 <see cref="Optional{T}"/> 对象。</returns>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="valueType"/> 为 null。
        /// </exception>
        public static object CreateOptional(Type valueType, object value)
        {
            var optionalType = typeof(Optional<>).MakeGenericType(valueType);
            var optional = Activator.CreateInstance(optionalType);
            SetOptionalValue(optional, value);
            return optional;
        }
    }
}
