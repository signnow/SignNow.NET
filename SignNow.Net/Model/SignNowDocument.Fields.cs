using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using SignNow.Net.Internal.Helpers;
using SignNow.Net.Internal.Model.FieldTypes;

namespace SignNow.Net.Model
{
    /// <inheritdoc />
    /// <remarks>
    /// This part contains related to Fields and Fields value retieval methods only.
    /// </remarks>
    public partial class SignNowDocument
    {
        /// <summary>
        /// All the document <see cref="TextField"/> fields.
        /// </summary>
        [JsonProperty("texts")]
        internal IReadOnlyCollection<TextField> Texts { get; private set; } = new List<TextField>();

        /// <summary>
        /// All the document <see cref="HyperlinkField"/> fields.
        /// </summary>
        [JsonProperty("hyperlinks")]
        internal IReadOnlyCollection<HyperlinkField> Hyperlinks { get; private set; } = new List<HyperlinkField>();

        /// <summary>
        /// All the documents <see cref="CheckboxField"/> fields.
        /// </summary>
        [JsonProperty("checks")]
        internal IReadOnlyCollection<CheckboxField> Checkboxes { get; private set; } = new List<CheckboxField>();

        /// <summary>
        /// All the documents <see cref="AttachmentField"/> fields.
        /// </summary>
        [JsonProperty("attachments")]
        internal IReadOnlyCollection<AttachmentField> Attachments { get; private set; } = new List<AttachmentField>();
        
        /// <summary>
        /// All the documents <see cref="EnumerationField"/> fields.
        /// </summary>
        [JsonProperty("enumeration_options")]
        internal IReadOnlyCollection<EnumerationField> Enumerations { get; private set; } = new List<EnumerationField>();

        /// <summary>
        /// Find Field value by <see cref="Field"/> metadata.
        /// </summary>
        /// <param name="fieldMeta">Field metadata.</param>
        /// <returns><see cref="object"/> with that represents state for <see cref="Field.Type"/></returns>
        public object GetFieldValue(Field fieldMeta)
        {
            Guard.PropertyNotNull(fieldMeta?.ElementId, "Cannot get field value without ElementId");

            switch (fieldMeta.Type)
            {
                case FieldType.Text:
                    return Texts.FirstOrDefault(txt => txt.Id == fieldMeta.ElementId);

                case FieldType.Signature:
                    return Signatures.FirstOrDefault(sig => sig.Id == fieldMeta.ElementId);

                case FieldType.Initial:
                    return Signatures.FirstOrDefault(sig => sig.Id == fieldMeta.ElementId);

                case FieldType.Hyperlink:
                    return Hyperlinks.FirstOrDefault(lnk => lnk.Id == fieldMeta.ElementId);

                case FieldType.Checkbox:
                    var checkbox = Checkboxes.FirstOrDefault(cbox => cbox.Id == fieldMeta.ElementId);
                    if (!string.IsNullOrEmpty(checkbox?.Id))
                    {
                        checkbox.Data = fieldMeta.JsonAttributes.PrefilledText == "1";
                    }

                    return checkbox;

                case FieldType.Attachment:
                    return Attachments.FirstOrDefault(atch => atch.Id == fieldMeta.ElementId);

                case FieldType.Dropdown:
                    return Texts.FirstOrDefault(drop => drop.Id == fieldMeta.ElementId);

                default:
                    return default;
            }
        }
    }
}
