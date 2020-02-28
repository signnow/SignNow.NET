using Newtonsoft.Json;
using SignNow.Net.Interfaces;

namespace SignNow.Net.Model.FieldContents
{
    /// <summary>
    /// Represents SignNow field types: `Attachment`
    /// </summary>
    public class AttachmentContent : BaseContent
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
        public string FileName { get; set; }

        /// <summary>
        /// File size in bytes.
        /// </summary>
        [JsonProperty("file_size")]
        public ulong FileSize { get; set; }

        /// <inheritdoc />
        public override object GetValue() => Id;
    }
}
