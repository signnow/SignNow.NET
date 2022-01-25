using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net.Internal.Requests;
using SignNow.Net.Model.EditFields;

namespace UnitTests
{
    [TestClass]
    public class EditFieldRequestTest
    {
        [TestMethod]
        public void ShouldProperCreateJsonRequest()
        {
            var field = new TextField
            {
                PageNumber = 0,
                Name = "TextName",
                Role = "Signer 1",
                Height = 100,
                Width = 200,
                Label = "LabelName",
                PrefilledText = "prefilled-text-example",
                Required = true,
                X = 10,
                Y = 20
            };

            var timestamp = new DateTime(2022, 1,1);
            var request = new EditFieldRequest(field)
            {
                ClientTime = timestamp
            };

            var actual = $@"{{
            ""client_timestamp"": ""1640995200"",
            ""fields"": [
                {{
                    ""type"": ""text"",
                    ""prefilled_text"": ""prefilled-text-example"",
                    ""label"": ""LabelName"",
                    ""page_number"": 0,
                    ""name"": ""TextName"",
                    ""role"": ""Signer 1"",
                    ""required"": true,
                    ""x"": 10,
                    ""y"": 20,
                    ""width"": 200,
                    ""height"": 100
                }}
            ]
            }}";

            Assert.That.JsonEqual(request, actual);
        }
    }
}
