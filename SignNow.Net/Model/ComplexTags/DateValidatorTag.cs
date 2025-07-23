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
        [JsonProperty("lock_to_sign_date", Order = 1)]
        public bool LockSigningDate { get; set; }

        /// <summary>
        /// Data validation format for a field
        /// </summary>
        [JsonProperty("validator_id", Order = 2)]
        [JsonConverter(typeof(StringEnumConverter))]
        public DataValidator Validator { get; set; }
    }
}
