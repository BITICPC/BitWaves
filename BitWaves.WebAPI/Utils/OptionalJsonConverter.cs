using System;
using Newtonsoft.Json;

namespace BitWaves.WebAPI.Utils
{
    /// <summary>
    /// 为 <see cref="Optional{T}"/> 提供 JSON 序列化逻辑。
    /// </summary>
    public sealed class OptionalJsonConverter : JsonConverter
    {
        private static Type ExtractOptionalValueType(Type optionalType)
        {
            return optionalType.GetGenericArguments()[0];
        }

        /// <inheritdoc />
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }

            if (!OptionalUtils.TryGetOptionalValue(value, out var optionalValue))
            {
                // 对于空的 Optional<T> 实例，向 JSON 输出写入 undefined
                writer.WriteUndefined();
                return;
            }

            serializer.Serialize(writer, optionalValue);
        }

        /// <inheritdoc />
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
                                        JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Undefined)
            {
                // undefined 表示空的 Optional<T> 实例。直接使用 Activator 实例化即可。
                existingValue = Activator.CreateInstance(objectType);
                return existingValue;
            }

            var valueType = ExtractOptionalValueType(objectType);
            var value = serializer.Deserialize(reader, valueType);

            if (existingValue != null && OptionalUtils.IsOptionalType(existingValue.GetType()))
            {
                OptionalUtils.SetOptionalValue(existingValue, value);
            }
            else
            {
                existingValue = OptionalUtils.CreateOptional(valueType, value);
            }

            return existingValue;
        }

        /// <inheritdoc />
        public override bool CanConvert(Type objectType)
        {
            return OptionalUtils.IsOptionalType(objectType);
        }
    }
}
