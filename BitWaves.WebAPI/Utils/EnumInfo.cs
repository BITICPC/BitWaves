using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;

namespace BitWaves.WebAPI.Utils
{
    /// <summary>
    /// 提供枚举类型的元信息以供 <see cref="EnumJsonConverter"/> 使用。
    /// </summary>
    internal sealed class EnumInfo
    {
        /// <summary>
        /// 初始化 <see cref="EnumInfo"/> 类的新实例。
        /// </summary>
        private EnumInfo()
        {
        }

        /// <summary>
        /// 获取枚举类型。
        /// </summary>
        public Type EnumType { get; private set; }

        /// <summary>
        /// 获取当前枚举类型中的所有枚举值。
        /// </summary>
        public IImmutableSet<Enum> Values { get; private set; }

        /// <summary>
        /// 获取枚举类型是否带有 <see cref="FlagsAttribute"/> 标签。
        /// </summary>
        public bool IsFlags { get; private set; }

        /// <summary>
        /// 获取枚举类型的底层类型是否是一个有符号类型。
        /// </summary>
        public bool IsSigned { get; private set; }

        /// <summary>
        /// 检查给定的枚举值类型与当前的枚举类型相匹配。
        /// </summary>
        /// <param name="value">要检查的枚举值。</param>
        /// <param name="variableName">要检查的枚举值在调用方位置处的名称。</param>
        /// <exception cref="ArgumentException">给定的枚举值与当前的枚举类型不匹配。</exception>
        private void EnsureTypeMatch(Enum value, string variableName)
        {
            if (value.GetType() != EnumType)
                throw new ArgumentException($"{variableName} 的类型与当前枚举类型不匹配。", variableName);
        }

        /// <summary>
        /// 测试给定的枚举值是否在当前的枚举中。
        /// </summary>
        /// <param name="value">要测试的值。</param>
        /// <returns>给定的枚举值是否在当前的枚举中。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> 为 null。</exception>
        public bool HasValue(Enum value)
        {
            Contract.NotNull(value, nameof(value));

            return Enum.IsDefined(EnumType, value);
        }

        /// <summary>
        /// 测试给定的枚举值是否可以由当前的枚举中的一个或多个枚举值导出且当前的枚举是一个 flags 枚举。
        /// </summary>
        /// <param name="value">要测试的值。</param>
        /// <returns>给定的枚举值是否可以由当前的枚举中的一个或多个枚举值导出且当前的枚举是一个 flags 枚举。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> 为 null。</exception>
        public bool HasFlags(Enum value)
        {
            Contract.NotNull(value, nameof(value));

            if (!IsFlags)
            {
                return HasValue(value);
            }

            var numeric = ToUInt64Unchecked(value);
            foreach (var e in Values.Select(ToUInt64Unchecked))
            {
                if ((numeric & e) == e)
                {
                    numeric ^= e;
                }

                if (numeric == 0)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 测试给定的枚举名称是否在当前的枚举中。
        /// </summary>
        /// <param name="name">要测试的枚举名称。</param>
        /// <returns>给定的枚举名称是否在当前的枚举中。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> 为 null。</exception>
        public bool HasName(string name)
        {
            Contract.NotNull(name, nameof(name));

            return Enum.IsDefined(EnumType, name);
        }

        /// <summary>
        /// 将给定的枚举值转换为 <see cref="ulong"/> 类型，忽略任何的数值溢出错误。
        /// </summary>
        /// <param name="value">要转换的枚举值。</param>
        /// <returns>转换后的枚举值。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> 为 null。</exception>
        /// <exception cref="ArgumentException"><paramref name="value"/> 的类型与当前枚举类型不匹配。</exception>
        public ulong ToUInt64Unchecked(Enum value)
        {
            Contract.NotNull(value, nameof(value));
            EnsureTypeMatch(value, nameof(value));

            if (IsSigned)
            {
                return unchecked((ulong) Convert.ToInt64(value));
            }
            else
            {
                return Convert.ToUInt64(value);
            }
        }

        /// <summary>
        /// 获取给定枚举值的各个 flag 枚举值。
        /// </summary>
        /// <param name="value">枚举值。</param>
        /// <returns>给定枚举值的各个 flag 枚举值。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> 为 null。</exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="value"/> 的类型与当前枚举值不匹配
        ///     或
        ///     <paramref name="value"/> 不能被当前枚举的各个值异或表出。
        /// </exception>
        public Enum[] GetComponents(Enum value)
        {
            Contract.NotNull(value, nameof(value));
            EnsureTypeMatch(value, nameof(value));

            if (!IsFlags)
            {
                if (!Enum.IsDefined(EnumType, value))
                    throw new ArgumentException($"{nameof(value)} 不是当前枚举类型的有效的枚举值。", nameof(value));
                return new[] { value };
            }

            var components = new List<Enum>();
            var numeric = ToUInt64Unchecked(value);

            foreach (var e in Values)
            {
                var ne = ToUInt64Unchecked(e);

                if ((numeric & ne) == ne)
                {
                    numeric ^= ne;
                    components.Add(e);
                }

                if (numeric == 0)
                {
                    return components.ToArray();
                }
            }

            throw new ArgumentException($"{nameof(value)} 不是有效的枚举值组合。", nameof(value));
        }

        /// <summary>
        /// 从给定的枚举类型创建 <see cref="EnumInfo"/> 实例。
        /// </summary>
        /// <param name="enumType">枚举类型。</param>
        /// <returns>包装与给定的枚举类型相对应的枚举信息的 <see cref="EnumInfo"/> 实例对象。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumType"/> 为 null。</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType"/> 不是一个枚举类型。</exception>
        public static EnumInfo From(Type enumType)
        {
            Contract.NotNull(enumType, nameof(enumType));

            if (!enumType.IsEnum)
                throw new ArgumentException("enumType 不是一个枚举类型。", nameof(enumType));

            var signedType = new[]
            {
                typeof(sbyte), typeof(short), typeof(int), typeof(long)
            };

            var ei = new EnumInfo
            {
                EnumType = enumType,
                Values = Enum.GetValues(enumType).Cast<Enum>().ToImmutableHashSet(),
                IsFlags = enumType.GetCustomAttribute(typeof(FlagsAttribute)) != null,
                IsSigned = signedType.Contains(Enum.GetUnderlyingType(enumType))
            };

            return ei;
        }
    }
}
