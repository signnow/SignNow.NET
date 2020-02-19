using Newtonsoft.Json;

namespace SignNow.Net.Internal.Model.FieldTypes
{
    /// <summary>
    /// Represents SignNow field types: `Attachment`
    /// </summary>
    internal class AttachmentField : BaseField
    {
        /// <summary>
        /// Original attachment file name with extension.
        /// </summary>
        [JsonProperty("original_attachment_name")]
        public string OriginalName { get; set; }

        /// <summary>
        /// Filename with extension used in SignNow.
        /// </summary>
        [JsonProperty("filename")]
        public string Filename { get; set; }

        /// <summary>
        /// File size in bytes.
        /// </summary>
        [JsonProperty("file_size")]
        public ulong FileSize { get; set; }
    }
}
