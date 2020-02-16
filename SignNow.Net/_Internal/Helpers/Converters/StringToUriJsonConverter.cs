using System;
using Newtonsoft.Json;

namespace SignNow.Net.Internal.Helpers.Converters
{
    /// <summary>
    /// Converts <see cref="String"/> to <see cref="Uri"/> to and from JSON.
    /// </summary>
    public class StringToUriJsonConverter : JsonConverter
    {
        /// <inheritdoc cref="JsonConverter.CanConvert(Type)" />
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Uri);
        }

        /// <inheritdoc cref="JsonConverter.ReadJson(JsonReader, Type, object, JsonSerializer)" />
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            Uri url = default;

            if (reader.TokenType == JsonToken.String)
            {
                Uri.TryCreate(reader.Value.ToString(), UriKind.Absolute, out url);
            }

            return url;
        }

        /// <inheritdoc cref="JsonConverter.WriteJson(JsonWriter, object, JsonSerializer)"/>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value == null)
            {
                writer.WriteNull();

                return;
            }

            writer.WriteValue(((Uri)value).OriginalString);
        }
    }
}
