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
                    return Texts.First(txt => txt.Id == fieldMeta.ElementId);

                case FieldType.Signature:
                    return Signatures.Find(sig => sig.Id == fieldMeta.ElementId);

                case FieldType.Hyperlink:
                    return Hyperlinks.First(lnk => lnk.Id == fieldMeta.ElementId);

                case FieldType.Checkbox:
                    return GetCheckboxItem(fieldMeta);

                default:
                    return default;
            }
        }

        private object GetCheckboxItem(Field fieldMeta)
        {
            try
            {
                var checkbox = Checkboxes.First(cbox => cbox.Id == fieldMeta.ElementId);
                checkbox.Data = fieldMeta.JsonAttributes.PrefilledText == "1";

                return checkbox;
            }
            catch (InvalidOperationException ex)
            {
                return (object)(new CheckboxField());
            }
        }
    }
}
