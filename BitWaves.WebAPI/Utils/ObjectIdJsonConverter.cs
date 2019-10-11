using System;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Newtonsoft.Json;

namespace BitWaves.WebAPI.Utils
{
    /// <summary>
    /// 为 <see cref="ObjectId"/> 提供 <see cref="JsonConverter"/> 实现。
    /// </summary>
    public sealed class ObjectIdJsonConverter : JsonConverter<ObjectId>
    {
        /// <inheritdoc />
        public override void WriteJson(JsonWriter writer, ObjectId value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value.ToString());
        }

        /// <inheritdoc />
        public override ObjectId ReadJson(JsonReader reader, Type objectType, ObjectId existingValue, bool hasExistingValue,
                                          JsonSerializer serializer)
        {
            var text = serializer.Deserialize<string>(reader);
            return ObjectId.Parse(text);
        }
    }

    /// <summary>
    /// 为 <see cref="ObjectIdJsonConverter"/> 提供扩展。
    /// </summary>
    public static class ObjectIdJsonConverterExtensions
    {
        /// <summary>
        /// 向给定的 MVC JSON 选项容器中注册 <see cref="ObjectIdJsonConverter"/>。
        /// </summary>
        /// <param name="options">MVC JSON 选项容器。</param>
        /// <returns>MVC JSON 选项容器。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="options"/> 为 null。</exception>
        public static MvcJsonOptions AddObjectIdConverter(this MvcJsonOptions options)
        {
            Contract.NotNull(options, nameof(options));

            options.SerializerSettings.Converters.Add(new ObjectIdJsonConverter());
            return options;
        }
    }
}
