using System;
using System.Globalization;
using Newtonsoft.Json;

namespace SignNow.Net.Internal.Helpers.Converters
{
    /// <summary>
    /// Converts <see cref="String"/> to <see cref="int"/> to and from JSON.
    /// </summary>
    internal class StringToIntJsonConverter : JsonConverter
    {
        /// <inheritdoc cref="JsonConverter.WriteJson" />
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteValue((int)value);
        }

        /// <inheritdoc cref="JsonConverter.ReadJson" />
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            return int.Parse(reader.Value.ToString().Trim(), CultureInfo.InvariantCulture);
        }

        /// <inheritdoc cref="JsonConverter.CanConvert" />
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(int);
        }
    }
}
