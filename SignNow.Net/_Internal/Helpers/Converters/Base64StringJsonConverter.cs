using System;
using Newtonsoft.Json;

namespace SignNow.Net.Internal.Helpers.Converters
{
    /// <summary>
    /// Converts Base64 <see cref="String"/> to <see cref="Byte[]"/> to and from JSON.
    /// </summary>
    public class Base64StringJsonConverter : JsonConverter
    {
        /// <inheritdoc cref="JsonConverter.CanConvert(Type)" />
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(byte[]);
        }

        /// <inheritdoc cref="JsonConverter.ReadJson(JsonReader, Type, object, JsonSerializer)" />
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return Convert.FromBase64String(reader.Value.ToString());
        }

        /// <inheritdoc cref="JsonConverter.WriteJson(JsonWriter, object, JsonSerializer)"/>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteValue(Convert.ToBase64String((byte[])value));
        }
    }
}
