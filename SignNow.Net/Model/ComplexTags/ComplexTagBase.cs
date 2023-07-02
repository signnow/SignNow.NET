using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SignNow.Net.Interfaces;

namespace SignNow.Net.Model.ComplexTags
{
    public abstract class ComplexTagBase
    {
        /// <summary>
        /// The name of the Tag
        /// </summary>
        [JsonProperty("tag_name")]
        public string TagName { get; set; }

        /// <summary>
        /// Which role the field is assigned to, e.g. Signer_1
        /// </summary>
        [JsonProperty("role")]
        public string Role { get; set; }

        /// <summary>
        /// Whether the field is mandatory to fill in
        /// </summary>
        [JsonProperty("required")]
        public bool Required { get; set; }

        /// <summary>
        /// Field type
        /// </summary>
        [JsonProperty("type")]
        [JsonConverter(typeof(StringEnumConverter))]
        public virtual FieldType Type { get; protected set; }

        /// <summary>
        /// How many pixels wide the field is
        /// </summary>
        [JsonProperty("width")]
        public int Width { get; set; }

        /// <summary>
        /// How many pixels high the field is
        /// </summary>
        [JsonProperty("height")]
        public int Height { get; set; }
    }

    public abstract class ComplexTagWithLabel : ComplexTagBase
    {
        /// <summary>
        /// optional) - hint for the signer inside a fillable field about the field type,
        /// e.g. first_name or text_1;
        /// once the field is filled in, the value automatically appears in all the fields with the same label
        /// </summary>
        [JsonProperty("label", NullValueHandling = NullValueHandling.Ignore)]
        public string Label { get; set; }
    }
}
