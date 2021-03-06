using System;
using BitWaves.Data.Utils;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BitWaves.WebAPI.Utils
{
    /// <summary>
    /// 为 <see cref="Maybe{T}"/> 提供 JSON 序列化逻辑。
    /// </summary>
    public sealed class MaybeJsonConverter : JsonConverter
    {
        /// <inheritdoc />
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }

            var maybeValue = MaybeUtils.Unbox(value);
            if (!maybeValue.HasValue)
            {
                // 对于空的 Optional<T> 实例，向 JSON 输出写入 undefined
                writer.WriteUndefined();
                return;
            }

            serializer.Serialize(writer, maybeValue.Value);
        }

        /// <inheritdoc />
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
                                        JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Undefined)
            {
                // undefined 表示空的 Maybe<T> 实例。直接使用 Activator 实例化即可。
                existingValue = Activator.CreateInstance(objectType);
                return existingValue;
            }

            var valueType = MaybeUtils.GetInnerType(objectType);
            var value = serializer.Deserialize(reader, valueType);

            if (existingValue != null && MaybeUtils.IsMaybe(existingValue))
            {
                MaybeUtils.SetInnerValue(existingValue, value);
            }
            else
            {
                existingValue = MaybeUtils.Create(valueType, value);
            }

            return existingValue;
        }

        /// <inheritdoc />
        public override bool CanConvert(Type objectType)
        {
            return MaybeUtils.IsMaybeType(objectType);
        }
    }

    /// <summary>
    /// 为 <see cref="MaybeJsonConverter"/> 提供扩展方法。
    /// </summary>
    public static class MaybeJsonConverterExtensions
    {
        /// <summary>
        /// 将 <see cref="MaybeJsonConverter"/> 注册到给定的 <see cref="MvcJsonOptions"/> 对象上。
        /// </summary>
        /// <param name="options"><see cref="MvcJsonOptions"/> 对象。</param>
        /// <exception cref="ArgumentNullException"><paramref name="options"/> 为 null。</exception>
        public static MvcJsonOptions AddMaybeJsonConverter(this MvcJsonOptions options)
        {
            Contract.NotNull(options, nameof(options));

            options.SerializerSettings.Converters.Add(new MaybeJsonConverter());
            return options;
        }
    }
}
