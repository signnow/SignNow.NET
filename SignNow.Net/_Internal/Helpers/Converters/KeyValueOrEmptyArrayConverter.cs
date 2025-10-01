using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SignNow.Net.Internal.Helpers.Converters
{
    public class KeyValueOrEmptyArrayConverter: JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Dictionary<string, object>);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.StartObject)
            {
                return serializer.Deserialize<Dictionary<string, object>>(reader);
            }

            if (reader.TokenType == JsonToken.StartArray)
            {
                JArray array = JArray.Load(reader);
                if (array.Count == 0)
                {
                    return new Dictionary<string, object>();
                }
            }

            throw new JsonSerializationException("Unexpected token type: " + reader.TokenType);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value is Dictionary<string, object> dictionary && dictionary.Count == 0)
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
