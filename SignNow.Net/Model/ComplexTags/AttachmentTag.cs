namespace SignNow.Net.Model.ComplexTags
{
    public class AttachmentTag : ComplexTagWithLabel
    {
        /// <inheritdoc />
        public override FieldType Type { get; protected set; } = FieldType.Attachment;
    }
}
