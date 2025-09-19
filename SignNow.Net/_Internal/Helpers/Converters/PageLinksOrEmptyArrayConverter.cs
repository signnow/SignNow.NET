using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SignNow.Net.Model;

namespace SignNow.Net.Internal.Helpers.Converters
{
    /// <summary>
    /// Converts PageLinks to handle cases where the API returns an empty array instead of an object.
    /// </summary>
    internal class PageLinksOrEmptyArrayConverter : JsonConverter
    {
        /// <inheritdoc cref="JsonConverter.CanConvert(Type)" />
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(PageLinks);
        }

        /// <inheritdoc cref="JsonConverter.ReadJson(JsonReader, Type, object, JsonSerializer)" />
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.StartObject)
            {
                return serializer.Deserialize<PageLinks>(reader);
            }

            if (reader.TokenType == JsonToken.StartArray)
            {
                JArray array = JArray.Load(reader);
                if (array.Count == 0)
                {
                    return new PageLinks();
                }
            }

            throw new JsonSerializationException("Unexpected token type: " + reader.TokenType);
        }

        /// <inheritdoc cref="JsonConverter.WriteJson(JsonWriter, object, JsonSerializer)" />
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value is PageLinks pageLinks && 
                pageLinks.Previous == null && 
                pageLinks.Next == null)
            {
                writer.WriteStartArray();
                writer.WriteEndArray();
            }
            else
            {
                serializer.Serialize(writer, value);
            }
        }
    }
}
