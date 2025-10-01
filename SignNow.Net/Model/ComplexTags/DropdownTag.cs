using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace SignNow.Net.Model.ComplexTags
{
    public class DropdownTag : ComplexTagWithLabel
    {
        /// <inheritdoc />
        [JsonProperty("type")]
        [JsonConverter(typeof(StringEnumConverter))]
        public override FieldType Type { get; protected set; } = FieldType.Enumeration;

        [JsonProperty("custom_defined_option")]
        public bool CustomDefinedOptions { get; set; }

        [JsonProperty("enumeration_options")]
        public List<string> EnumerationOptions { get; set; } = new List<string>();
    }
}
