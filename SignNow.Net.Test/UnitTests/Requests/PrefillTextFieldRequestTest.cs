using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net.Internal.Requests;
using SignNow.Net.Model.EditFields;
using SignNow.Net.Test.FakeModels.EditFields;

namespace UnitTests
{
    [TestClass]
    public class PrefillTextFieldRequestTest
    {
        [TestMethod]
        public void ShouldProperCreateJsonRequest()
        {
            var textField = new TextField { Name = "Text_1", PrefilledText = "prefilled-text" };

            var request1 = new PrefillTextFieldRequest(textField);
            var request2 = new PrefillTextFieldRequest(new TextFieldFaker().Generate(5));

            var actual1 = $@"{{
            ""fields"": [
                {{
                    ""field_name"": ""Text_1"",
                    ""prefilled_text"": ""prefilled-text""
                }}
            ]
            }}";

            Assert.That.JsonEqual(request1, actual1);
            Assert.AreEqual(5, request2.Fields.Count);
            Assert.IsTrue(request2.Fields.TrueForAll(itm => itm.FieldName.StartsWith("Text_")));
        }
    }
}
