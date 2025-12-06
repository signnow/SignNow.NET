using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SignNow.Net._Internal.Helpers.Converters
{
    internal class ObjectOrEmptyArrayConverter : JsonConverter
    {
        /// <inheritdoc cref="JsonConverter.CanConvert(Type)" />
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(object); // todo
        }

        /// <inheritdoc cref="JsonConverter.ReadJson(JsonReader, Type, object, JsonSerializer)" />
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.StartObject)
            {
                return serializer.Deserialize(reader, objectType);
            }

            if ((reader.TokenType == JsonToken.StartArray && JArray.Load(reader).Count == 0) || reader.TokenType == JsonToken.Null)
            {
                return null;
            }

            throw new JsonSerializationException("Unexpected token type: " + reader.TokenType);
        }

        /// <inheritdoc cref="JsonConverter.WriteJson(JsonWriter, object, JsonSerializer)" />
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value);
        }
    }
}
