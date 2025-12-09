using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SignNow.Net._Internal.Helpers.Converters
{
    /// <summary>
    /// A JSON converter that handles properties that can be either a JSON object or an empty array.
    /// When the property is an empty array or null, it converts to null; otherwise, it deserializes as the target type.
    /// </summary>
    internal class ObjectOrEmptyArrayConverter : JsonConverter
    {
        /// <summary>
        /// Determines whether this converter can convert the specified object type.
        /// </summary>
        /// <param name="objectType">The type of object to check for conversion compatibility.</param>
        /// <returns><c>true</c> if this converter can convert the specified type; otherwise, <c>false</c>.</returns>
        /// <remarks>
        /// This converter is designed to work with any reference type that might be represented 
        /// as either an object or an empty array in JSON. It excludes value types and strings
        /// as they typically don't follow this pattern.
        /// </remarks>
        public override bool CanConvert(Type objectType)
        {
            return objectType != null && 
                   !objectType.IsValueType && 
                   objectType != typeof(string);
        }

        /// <summary>
        /// Reads the JSON representation of the object.
        /// </summary>
        /// <returns>
        /// The deserialized object if the JSON represents a non-empty object,
        /// or <c>null</c> if the JSON is an empty array or null.
        /// </returns>
        /// <exception cref="JsonSerializationException">
        /// Thrown when the JSON token is neither an object, empty array, nor null.
        /// </exception>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            switch (reader.TokenType)
            {
                case JsonToken.StartObject:
                    return serializer.Deserialize(reader, objectType);

                case JsonToken.StartArray:
                    if (JArray.Load(reader).Count == 0)
                        return null;
                    
                    throw new JsonSerializationException($"Cannot convert non-empty array to type '{objectType?.Name ?? "unknown"}'. Only empty arrays are supported for conversion to null.");

                case JsonToken.Null:
                    return null;

                default:
                    throw new JsonSerializationException(
                        $"Unexpected JSON token '{reader.TokenType}' when reading type '{objectType?.Name ?? "unknown"}'. Expected StartObject, empty StartArray, or Null.");
            }
        }

        /// <summary>
        /// Writes the JSON representation of the object using their standard JSON representation
        /// </summary>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value);
        }
    }
}
