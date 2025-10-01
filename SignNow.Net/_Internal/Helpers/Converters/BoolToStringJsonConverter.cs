using System;
using Newtonsoft.Json;

namespace SignNow.Net.Internal.Helpers.Converters
{
    /// <summary>
    /// Converts <see cref="System.Boolean"/> to <see cref="int"/>
    /// </summary>
    internal class BoolToStringJsonConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var strValue = value != null && (bool)value == true ? "true" : "false";
            writer.WriteValue(strValue);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return reader.Value?.ToString().ToLower() == "true";
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(bool);
        }
    }
}
