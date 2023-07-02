namespace SignNow.Net.Model.ComplexTags
{
    public class CheckBoxTag : ComplexTagBase
    {
        /// <inheritdoc />
        public override FieldType Type { get; protected set; } = FieldType.Checkbox;
    }
}
