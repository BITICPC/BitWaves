using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BitWaves.WebAPI.Utils
{
    /// <summary>
    /// 为应用程序提供 <see cref="Nullable{T}"/> 类型的 JSON 转换逻辑。
    /// </summary>
    public sealed class NullableJsonConverter : JsonConverter
    {
        /// <inheritdoc />
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }

            // We still consider the case when `value` is not a Nullable<T> object due to a possible bug in
            // Newtonsoft.Json that Nullable<T> values are automatically unwrapped when passing such values
            // to the WriteJson method of custom converters.

            if (IsNullable(value))
            {
                serializer.Serialize(writer, GetNullableInnerValue(value));
            }
            else
            {
                serializer.Serialize(writer, value);
            }
        }

        /// <inheritdoc />
        /// <remarks>
        /// This converter is not intended for deserializing <see cref="Nullable{T}"/> values. The builtin converter in
        /// Newtonsoft.Json can handle it pretty well.
        /// </remarks>
        public override bool CanRead => false;

        /// <inheritdoc />
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
                                        JsonSerializer serializer)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// 检查给定的类型是否为 <see cref="Nullable{T}"/> 类型。
        /// </summary>
        /// <param name="type">要检查的类型。</param>
        /// <returns>给定的类型是否为 <see cref="Nullable{T}"/> 类型。</returns>
        private static bool IsNullableType(Type type)
        {
            return type.IsConstructedGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
        }

        /// <summary>
        /// 检查给定的对象是否为 <see cref="Nullable{T}"/> 类型的对象。
        /// </summary>
        /// <param name="value">要检查的对象。</param>
        /// <returns>给定的对象的类型是否为 <see cref="Nullable{T}"/> 类型。</returns>
        private static bool IsNullable(object value)
        {
            return IsNullableType(value.GetType());
        }

        /// <summary>
        /// 检查给定的类型是否为 <see cref="Nullable{T}"/> 类型。若给定的类型不是 <see cref="Nullable{T}"/> 类型，该方法抛出
        /// <see cref="InvalidOperationException"/> 异常。
        /// </summary>
        /// <param name="type">要检查的类型。</param>
        /// <exception cref="InvalidOperationException">给定的类型不是 <see cref="Nullable{T}"/> 类型。</exception>
        private static void EnsureIsNullableType(Type type)
        {
            if (!IsNullableType(type))
                throw new InvalidOperationException("The given type is not a nullable type.");
        }

        /// <summary>
        /// 检查给定的对象是否为 <see cref="Nullable{T}"/> 类型。若给定的对象不是 <see cref="Nullable{T}"/> 类型，该方法抛出
        /// <see cref="InvalidOperationException"/> 异常。
        /// </summary>
        /// <param name="value">要检查的对象。</param>
        /// <exception cref="InvalidOperationException">给定的对象不是 <see cref="Nullable{T}"/> 类型。</exception>
        private static void EnsureIsNullable(object value)
        {
            EnsureIsNullableType(value.GetType());
        }

        /// <summary>
        /// 获取 <see cref="Nullable{T}"/> 类型的内部类型。
        /// </summary>
        /// <param name="nullableType">一个 <see cref="Nullable{T}"/> 类型。</param>
        /// <returns>给定的 <see cref="Nullable{T}"/> 类型的内部类型。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="nullableType"/> 为 null。</exception>
        /// <exception cref="InvalidOperationException">
        /// <paramref name="nullableType"/> 不是一个 <see cref="Nullable{T}"/> 类型。
        /// </exception>
        private static Type GetNullableInnerType(Type nullableType)
        {
            Contract.NotNull(nullableType, nameof(nullableType));
            EnsureIsNullableType(nullableType);

            return nullableType.GetGenericArguments()[0];
        }

        /// <summary>
        /// 获取给定的 <see cref="Nullable{T}"/> 值的内部值。
        /// </summary>
        /// <param name="nullableValue">一个 <see cref="Nullable{T}"/> 类型的值。</param>
        /// <returns>给定的 <see cref="Nullable{T}"/> 值的内部值。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="nullableValue"/> 为 null。</exception>
        /// <exception cref="InvalidOperationException">
        /// <paramref name="nullableValue"/> 不是一个 <see cref="Nullable{T}"/> 类型的对象。
        /// </exception>
        private static object GetNullableInnerValue(object nullableValue)
        {
            Contract.NotNull(nullableValue, nameof(nullableValue));
            EnsureIsNullable(nullableValue);

            var nullableHasValueProperty = nullableValue.GetType().GetProperty("HasValue");
            var nullableValueProperty = nullableValue.GetType().GetProperty("Value");
            Debug.Assert(nullableHasValueProperty != null, "nullableHasValueProperty != null");
            Debug.Assert(nullableValueProperty != null, "nullableValueProperty != null");

            var hasValue = (bool) nullableHasValueProperty.GetValue(nullableValue);
            if (!hasValue)
            {
                return null;
            }

            return nullableValueProperty.GetValue(nullableValue);
        }

        /// <inheritdoc />
        public override bool CanConvert(Type objectType)
        {
            return IsNullableType(objectType);
        }
    }

    /// <summary>
    /// 为 <see cref="NullableJsonConverter"/> 提供扩展方法。
    /// </summary>
    public static class NullableJsonConverterExtensions
    {
        /// <summary>
        /// 将 <see cref="NullableJsonConverter"/> 注册为全局的 JSON 转换器。
        /// </summary>
        /// <param name="options">JSON 选项。</param>
        /// <exception cref="ArgumentNullException"><paramref name="options"/> 为 null。</exception>
        public static MvcJsonOptions AddNullableJsonConverter(this MvcJsonOptions options)
        {
            Contract.NotNull(options, nameof(options));

            options.SerializerSettings.Converters.Add(new NullableJsonConverter());
            return options;
        }
    }
}
