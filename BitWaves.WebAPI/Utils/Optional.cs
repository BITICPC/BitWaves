using Newtonsoft.Json;

namespace BitWaves.WebAPI.Utils
{
    /// <summary>
    /// 表示一个可能含有给定类型的值的容器。
    /// </summary>
    /// <typeparam name="T">要承载的值的类型。</typeparam>
    [JsonConverter(typeof(OptionalJsonConverter))]
    public struct Optional<T>
    {
        private T _value;
        private bool _hasValue;

        /// <summary>
        /// 初始化 <see cref="Optional{T}"/> 的新实例。该实例中包含给定的值。
        /// </summary>
        /// <param name="value">要承载的值。</param>
        public Optional(T value)
        {
            _value = value;
            _hasValue = true;
        }

        /// <summary>
        /// 获取当前 <see cref="Optional{T}"/> 中包含的值。
        /// </summary>
        /// <exception cref="EmptyOptionalException">
        /// 当前的 <see cref="Optional{T}"/> 实例不包含任何值。
        /// </exception>
        public T Value
        {
            get
            {
                if (!_hasValue)
                    throw new EmptyOptionalException();

                return _value;
            }
        }

        /// <summary>
        /// 测试当前的 <see cref="Optional{T}"/> 实例中是否包含任何值。
        /// </summary>
        public bool HasValue => _hasValue;

        /// <summary>
        /// 尝试获取当前的 <see cref="Optional{T}"/> 中所包含的值。
        /// </summary>
        /// <param name="value">输出参数，若当前的 <see cref="Optional{T}"/> 中包含值，则该参数输出包含的值。</param>
        /// <returns>当前的 <see cref="Optional{T}"/> 中是否包含任何值。</returns>
        public bool TryGetValue(out T value)
        {
            value = _value;
            return _hasValue;
        }

        /// <summary>
        /// 设置当前的 <see cref="Optional{T}"/> 中所包含的值。
        /// </summary>
        /// <param name="value">要包含的值。</param>
        public void Set(T value)
        {
            _value = value;
            _hasValue = true;
        }

        /// <summary>
        /// 释放当前的 <see cref="Optional{T}"/> 中所包含的值。执行该操作后，当前的 <see cref="Optional{T}"/> 实例为空。
        /// </summary>
        public void Release()
        {
            _value = default;
            _hasValue = false;
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (!(obj is Optional<T> optionalValue))
            {
                return false;
            }

            return this == optionalValue;
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// 测试两个 <see cref="Optional{T}"/> 的实例是否相等。
        /// </summary>
        /// <param name="lhs">左操作数。</param>
        /// <param name="rhs">右操作数。</param>
        /// <returns>两个给定的 <see cref="Optional{T}"/> 实例是否相等。</returns>
        public static bool operator ==(Optional<T> lhs, Optional<T> rhs)
        {
            if (!lhs._hasValue && !rhs._hasValue)
            {
                return true;
            }

            if (!lhs._hasValue || !rhs._hasValue)
            {
                return false;
            }

            // _hasValue && optionalValue._hasValue
            return Equals(lhs._value, rhs._value);
        }

        /// <summary>
        /// 测试两个 <see cref="Optional{T}"/> 的实例是否不相等。
        /// </summary>
        /// <param name="lhs">左操作数。</param>
        /// <param name="rhs">右操作数。</param>
        /// <returns>两个给定的 <see cref="Optional{T}"/> 实例是否不相等。</returns>
        public static bool operator !=(Optional<T> lhs, Optional<T> rhs)
        {
            return !(lhs == rhs);
        }
    }
}
