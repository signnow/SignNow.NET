using System;
using System.Globalization;
using Newtonsoft.Json;
using SignNow.Net.Exceptions;

namespace SignNow.Net._Internal.Helpers.Converters
{
    internal class DurationToTimeSpanConverter : JsonConverter
    {
        /// <inheritdoc cref="JsonConverter.WriteJson" />
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteValue(((TimeSpan)value).TotalSeconds);
        }

        /// <inheritdoc cref="JsonConverter.ReadJson" />
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return reader.TokenType switch
            {
                JsonToken.Null => TimeSpan.FromSeconds(0),

                JsonToken.Integer => TimeSpan.FromSeconds((long)reader.Value),

                JsonToken.Float => TimeSpan.FromSeconds((double)reader.Value),

                _ => throw new JsonSerializationException(string.Format(
                        CultureInfo.CurrentCulture, ExceptionMessages.UnexpectedValueWhenConverting,
                        objectType.Name, "`Integer`, `Float`", reader.Value?.GetType().Name))
            };
        }

        /// <inheritdoc cref="JsonConverter.CanConvert" />
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(TimeSpan);
        }
    }
}
