using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using SignNow.Net.Model;
using SignNow.Net.Test.FakeModels;

namespace UnitTests
{
    [TestClass]
    public class FieldTest
    {
        [DataTestMethod]
        [DataRow(FieldType.Attachment, DisplayName = "with Attachment fields")]
        [DataRow(FieldType.Checkbox, DisplayName = "with Checkbox fields")]
        [DataRow(FieldType.Enumeration, DisplayName = "with Enumeration fields")]
        [DataRow(FieldType.Hyperlink, DisplayName = "with Hyperlink fields")]
        [DataRow(FieldType.Initials, DisplayName = "with Initials fields")]
        [DataRow(FieldType.Signature, DisplayName = "with Signature fields")]
        [DataRow(FieldType.Text, DisplayName = "with Text fields")]
        [DataRow(FieldType.RadioButton, DisplayName = "with Radiobutton fields")]
        public void ShouldDeserializeFromJson(FieldType type)
        {
            var fieldFake = new FieldFaker()
                .RuleFor(o => o.Type, type)
                .Generate();

            var expected =  JsonConvert.SerializeObject(fieldFake, Formatting.Indented);
            var fieldActual = JsonConvert.DeserializeObject<Field>(expected);

            Assert.That.JsonEqual(expected, fieldActual);
        }
    }
}
