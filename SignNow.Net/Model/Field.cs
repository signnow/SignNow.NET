using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace SignNow.Net.Model
{
    /// <summary>
    /// SignNow fields metadata.
    /// </summary>
    public class Field
    {
        /// <summary>
        /// Unique identifier of field.
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// Field type.
        /// </summary>
        [JsonProperty("type")]
        [JsonConverter(typeof(StringEnumConverter))]
        public FieldType Type { get; set; }

        /// <summary>
        /// <see cref="SignNow.Net.Model.Role"/> identity.
        /// </summary>
        /// TODO: Use Role model instead of RoleId + RoleName
        [JsonProperty("role_id")]
        public string RoleId { get; set; }

        /// <summary>
        /// Signer role name.
        /// </summary>
        [JsonProperty("role")]
        public string RoleName { get; set; }

        /// <summary>
        /// Field attributes: name, label, x/y coordinates, width, heigth...
        /// </summary>
        [JsonProperty("json_attributes")]
        public FieldJsonAttributes JsonAttributes { get; set; }

        /// <summary>
        /// Document owner email.
        /// </summary>
        [JsonProperty("originator")]
        public string Owner { get; set; }

        /// <summary>
        /// Signer email.
        /// </summary>
        [JsonProperty("fulfiller")]
        public string Signer { get; set; }

        /// <summary>
        /// Identity of specific element for corresponding field type.
        /// </summary>
        [JsonProperty("element_id")]
        public string ElementId { get; set; }
    }

    /// <summary>
    /// Represents all types of SignNow fields.
    /// </summary>
    public enum FieldType
    {
        /// <summary>
        /// Text box, Dropdown box, Data-Time picker.
        /// </summary>
        Text,

        /// <summary>
        /// Signature fields.
        /// </summary>
        Signature,

        /// <summary>
        /// Initials fields.
        /// </summary>
        Initial,

        /// <summary>
        /// Check box.
        /// </summary>
        Checkbox,

        /// <summary>
        /// Radio button group with Radio elements included.
        /// </summary>
        RadioButton,

        /// <summary>
        /// Document's attachment which can be downloaded by URL.
        /// </summary>
        Attachment,

        /// <summary>
        /// Hyperlink field with Url and label.
        /// </summary>
        Hyperlink
    }
}
