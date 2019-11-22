using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BitWaves.WebAPI.Utils
{
    /// <summary>
    /// 为应用程序提供枚举类型的 JSON 转换器。
    /// </summary>
    public sealed class EnumJsonConverter : JsonConverter
    {
        /// <inheritdoc />
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var enumValue = (Enum) value;
            var enumInfo = EnumInfo.From(enumValue.GetType());

            if (!enumInfo.HasFlags(enumValue))
            {
                // enumValue 无法用枚举值进行异或表出。此时应使用 enumValue 的数值进行序列化。
                if (enumInfo.IsSigned)
                {
                    serializer.Serialize(writer, Convert.ToInt64(enumValue));
                }
                else
                {
                    serializer.Serialize(writer, Convert.ToUInt64(enumValue));
                }

                return;
            }

            if (!enumInfo.IsFlags)
            {
                serializer.Serialize(writer, enumValue.ToString());
                return;
            }

            var components = enumInfo.GetComponents(enumValue);
            if (components.Length == 1)
            {
                serializer.Serialize(writer, components[0].ToString());
            }
            else
            {
                serializer.Serialize(writer, components.Select(e => e.ToString()));
            }
        }

        /// <inheritdoc />
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
                                        JsonSerializer serializer)
        {
            if (existingValue == null)
            {
                existingValue = serializer.Deserialize(reader);
            }

            return GetEnumFromExistingValue(objectType, existingValue);
        }

        /// <summary>
        /// 将给定的对象转换为给定类型的枚举值。
        /// </summary>
        /// <param name="enumType">枚举对象类型。</param>
        /// <param name="existingValue">已有的对象。</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="enumType"/> 为 null
        ///     或
        ///     <paramref name="existingValue"/> 为 null。
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///     <paramref name="existingValue"/> 是一个字符串序列但是枚举类型不是 flags 枚举
        ///     或
        ///     无法将 <paramref name="existingValue"/> 的类型转换到给定的枚举类型。
        /// </exception>
        private static object GetEnumFromExistingValue(Type enumType, object existingValue)
        {
            Contract.NotNull(enumType, nameof(enumType));
            Contract.NotNull(existingValue, nameof(existingValue));

            var enumInfo = EnumInfo.From(enumType);

            var existingType = existingValue.GetType();
            if (existingType.IsEnum)
            {
                return GetEnumFromExistingEnum(enumType, (Enum) existingValue);
            }

            if (existingType == typeof(string))
            {
                return GetEnumFromExistingString(enumType, (string) existingValue);
            }

            if (existingType.IsPrimitive &&
                existingType != typeof(bool) &&
                existingType != typeof(float) &&
                existingType != typeof(double))
            {
                return GetEnumFromExistingInteger(enumType, existingValue, enumInfo);
            }

            if (existingType.GetInterfaces().Contains(typeof(IEnumerable<string>)))
            {
                return GetEnumFromExistingStrings(enumType, (IEnumerable<string>) existingValue, enumInfo);
            }

            if (existingValue.GetType().IsSubclassOf(typeof(JToken)))
            {
                return GetEnumFromExistingJToken(enumType, (JToken) existingValue, enumInfo);
            }

            throw new InvalidOperationException(
                $"Enum of type {enumType} cannot be constructed from object of type {existingType}.");
        }

        private static object GetEnumFromExistingStrings(Type enumType, IEnumerable<string> existingValue,
                                                         EnumInfo enumInfo)
        {
            if (!enumInfo.IsFlags)
            {
                // Non-flags enumeration cannot be constructed from a string array.
                throw new InvalidOperationException(
                    "Non-flags enumeration cannot be constructed from a string array.");
            }

            var numeric = existingValue.Select(name => (Enum) Enum.Parse(enumType, name))
                                       .Select(enumInfo.ToUInt64Unchecked)
                                       .Aggregate((lhs, rhs) => lhs | rhs);
            return Enum.ToObject(enumType, numeric);
        }

        private static object GetEnumFromExistingString(Type enumType, string existingValue)
        {
            return Enum.Parse(enumType, existingValue);
        }

        private static object GetEnumFromExistingInteger(Type enumType, object existingValue, EnumInfo enumInfo)
        {
            if (enumInfo.IsSigned)
            {
                return Enum.ToObject(enumType, Convert.ToInt64(existingValue));
            }
            else
            {
                return Enum.ToObject(enumType, Convert.ToUInt64(existingValue));
            }
        }

        private static object GetEnumFromExistingEnum(Type enumType, Enum existingValue)
        {
            if (existingValue.GetType() != enumType)
                throw new InvalidOperationException(
                    $"Cannot construct an enum of type {enumType} from another enum type {existingValue.GetType()}");

            return existingValue;
        }

        private static object GetEnumFromExistingJToken(Type enumType, JToken existingValue, EnumInfo enumInfo)
        {
            switch (existingValue.Type)
            {
                case JTokenType.Integer:
                    if (enumInfo.IsSigned)
                    {
                        return GetEnumFromExistingInteger(enumType, existingValue.ToObject<long>(), enumInfo);
                    }
                    else
                    {
                        return GetEnumFromExistingInteger(enumType, existingValue.ToObject<ulong>(), enumInfo);
                    }

                case JTokenType.String:
                    return GetEnumFromExistingString(enumType, existingValue.ToObject<string>());

                case JTokenType.Array:
                    return GetEnumFromExistingStrings(enumType, existingValue.ToObject<string[]>(), enumInfo);

                default:
                    throw new InvalidOperationException(
                        $"Cannot construct enum type {enumType} from JToken type: {existingValue.Type}.");
            }
        }

        /// <inheritdoc />
        public override bool CanConvert(Type objectType)
        {
            return objectType.IsEnum;
        }
    }

    /// <summary>
    /// 为 <see cref="EnumJsonConverter"/> 提供扩展逻辑。
    /// </summary>
    public static class EnumJsonConverterExtensions
    {
        /// <summary>
        /// 将 <see cref="EnumJsonConverter"/> 添加到给定的 <see cref="MvcJsonOptions"/> 中。
        /// </summary>
        /// <param name="options"><see cref="MvcJsonOptions"/> 实例对象。</param>
        /// <returns>传入的 <see cref="MvcJsonOptions"/> 实例对象。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="options"/> 为 null。</exception>
        public static MvcJsonOptions AddEnumJsonConverter(this MvcJsonOptions options)
        {
            Contract.NotNull(options, nameof(options));

            options.SerializerSettings.Converters.Add(new EnumJsonConverter());
            return options;
        }
    }
}
