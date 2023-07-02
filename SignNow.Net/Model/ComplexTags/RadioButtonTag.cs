using Newtonsoft.Json;

namespace SignNow.Net.Model.ComplexTags
{
    public class RadioButtonTag : ComplexTag
    {
        /// <inheritdoc />
        public override FieldType Type { get; protected set; } = FieldType.RadioButton;

        /// <summary>
        /// Radiobutton name
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        // public List<RadioGroup>
    }
}
