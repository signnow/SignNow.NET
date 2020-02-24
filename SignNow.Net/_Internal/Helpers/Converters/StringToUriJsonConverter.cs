using System;
using System.Globalization;
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
            if (reader.TokenType == JsonToken.String && Uri.TryCreate(reader.Value.ToString(), UriKind.Absolute, out Uri url))
            {
                return url;
            }

            throw new JsonSerializationException(string.Format(
                        CultureInfo.CurrentCulture,
                        "Unexpected value when converting to Uri. Expected an absolute Url, got '{0}'.",
                        reader.Value.ToString()));
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
