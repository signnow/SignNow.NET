using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace SignNow.Net.Model.ComplexTags
{
    public class AttachmentTag : ComplexTagWithLabel
    {
        /// <inheritdoc />
        [JsonProperty("type")]
        [JsonConverter(typeof(StringEnumConverter))]
        public override FieldType Type { get; protected set; } = FieldType.Attachment;
    }
}
