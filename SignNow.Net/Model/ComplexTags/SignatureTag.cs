namespace SignNow.Net.Model.ComplexTags
{
    public class SignatureTag : ComplexTagBase
    {
        /// <inheritdoc />
        public override FieldType Type { get; protected set; } = FieldType.Signature;
    }
}
