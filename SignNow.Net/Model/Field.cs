using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SignNow.Net.Interfaces;
using SignNow.Net.Model.FieldContents;

namespace SignNow.Net.Model
{
    /// <summary>
    /// signNow fields metadata.
    /// </summary>
    public class Field: ISignNowField
    {
        /// <summary>
        /// Unique identifier of field.
        /// </summary>
        [JsonProperty("id")]
        internal string Id { get; set; }

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
        internal string RoleId { get; set; }

        /// <summary>
        /// Signer role name.
        /// </summary>
        [JsonProperty("role")]
        internal string RoleName { get; set; }

        /// <summary>
        /// Field attributes: name, label, x/y coordinates, width, height...
        /// </summary>
        [JsonProperty("json_attributes")]
        public FieldJsonAttributes JsonAttributes { get; set; }

        /// <summary>
        /// Document owner email.
        /// </summary>
        [JsonProperty("originator")]
        internal string Owner { get; set; }

        /// <summary>
        /// Signer email.
        /// </summary>
        [JsonProperty("fulfiller")]
        internal string Signer { get; set; }

        /// <summary>
        /// Field request ID
        /// </summary>
        [JsonProperty("field_request_id")]
        public string FieldRequestId { get; internal set; }

        /// <summary>
        /// Identity of specific element for corresponding field type.
        /// </summary>
        [JsonProperty("element_id")]
        public string ElementId { get; set; }

        /// <summary>
        /// Radio group elements initial state for Radiobuttons field type.
        /// </summary>
        [JsonProperty("radio", NullValueHandling = NullValueHandling.Ignore)]
        internal IReadOnlyCollection<RadioContent> RadioGroup { get; set; }
    }

    /// <summary>
    /// Represents all types of signNow fields.
    /// </summary>
    public enum FieldType
    {
        /// <summary>
        /// Text box, Dropdown box, Data-Time picker.
        /// </summary>
        [EnumMember(Value = "text")]
        Text,

        /// <summary>
        /// Signature fields.
        /// </summary>
        [EnumMember(Value = "signature")]
        Signature,

        /// <summary>
        /// Stamp fields.
        /// </summary>
        [EnumMember(Value = "stamp")]
        Stamp,

        /// <summary>
        /// Initials fields.
        /// </summary>
        [EnumMember(Value = "initials")]
        Initials,

        /// <summary>
        /// Check box.
        /// </summary>
        [EnumMember(Value = "checkbox")]
        Checkbox,

        /// <summary>
        /// Enumeration list.
        /// </summary>
        [EnumMember(Value = "enumeration")]
        Enumeration,

        /// <summary>
        /// Radio button group with Radio elements included.
        /// </summary>
        [EnumMember(Value = "radiobutton")]
        RadioButton,

        /// <summary>
        /// Document's attachment which can be downloaded by URL.
        /// </summary>
        [EnumMember(Value = "attachment")]
        Attachment,

        /// <summary>
        /// Hyperlink field with Url and label.
        /// </summary>
        [EnumMember(Value = "hyperlink")]
        Hyperlink
    }
}
