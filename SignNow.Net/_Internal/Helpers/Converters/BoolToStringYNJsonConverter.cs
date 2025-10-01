using System;
using Newtonsoft.Json;

namespace SignNow.Net.Internal.Helpers.Converters
{
    /// <summary>
    /// Converts <see cref="System.Boolean"/> to <see cref="int"/>
    /// </summary>
    internal class BoolToStringYNJsonConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var toYesNo = (bool)value == true ? "y" : "n";
            writer.WriteValue(toYesNo);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return reader.Value?.ToString() == "y";
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(bool);
        }
    }
}
