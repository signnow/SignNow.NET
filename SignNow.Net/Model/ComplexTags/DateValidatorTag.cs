using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SignNow.Net.Internal.Helpers.Converters;

namespace SignNow.Net.Model.ComplexTags
{
    public class DateValidatorTag : ComplexTagWithLabel
    {
        /// <summary>
        /// Field type
        /// </summary>
        [JsonProperty("type")]
        [JsonConverter(typeof(StringEnumConverter))]
        public override FieldType Type { get; protected set; } = FieldType.Text;

        /// <summary>
        /// Lock Signing Date option
        /// </summary>
        [JsonProperty("lsd", Order = 1)]
        [JsonConverter(typeof(BoolToStringYNJsonConverter))]
        public bool LockSigningDate { get; set; }

        [JsonProperty("validator_id", Order = 2)]
        public string Validator { get; set; }
    }
}
