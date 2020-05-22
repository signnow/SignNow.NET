using System;
using System.Globalization;
using Newtonsoft.Json;
using SignNow.Net.Exceptions;

namespace SignNow.Net.Internal.Helpers.Converters
{
    /// <summary>
    /// Converts <see cref="String"/> to <see cref="Boolean"/> to and from JSON.
    /// </summary>
    internal class StringToBoolJsonConverter : JsonConverter
    {
        /// <inheritdoc cref="JsonConverter.WriteJson" />
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteValue((bool)value);
        }

        /// <inheritdoc cref="JsonConverter.ReadJson" />>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            switch (reader.Value.ToString().ToUpperInvariant().Trim())
            {
                case "TRUE":
                case "1":
                    return true;
                case "FALSE":
                case "0":
                    return false;
                default:
                    throw new JsonSerializationException(string.Format(
                        CultureInfo.CurrentCulture, ExceptionMessages.UnexpectedValueWhenConverting,
                        objectType.Name, "`true`, `false`",
                        reader.Value.ToString()));
            }
        }

        /// <inheritdoc cref="JsonConverter.CanConvert" />
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(bool);
        }
    }
}
