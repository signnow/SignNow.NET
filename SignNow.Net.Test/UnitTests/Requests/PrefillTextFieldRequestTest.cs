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

        [TestMethod]
        public void CreatesJsonContext()
        {
            var fields = new TextFieldFaker().Generate(5);
            var request = new PrefillTextFieldRequest(fields);

            var jsonHttpContext = request.GetHttpContent();
            var jsonBody = jsonHttpContext.ReadAsStringAsync().Result;

            Assert.AreEqual("application/json", jsonHttpContext.Headers.ContentType?.MediaType);
            Assert.IsTrue(jsonBody.Contains("fields"));
            Assert.IsTrue(jsonBody.Contains("field_name"));
            Assert.IsTrue(jsonBody.Contains(fields[1].PrefilledText));
        }
    }
}
