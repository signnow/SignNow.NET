using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net.Interfaces;
using SignNow.Net.Internal.Requests;
using SignNow.Net.Model.EditFields;
using SignNow.Net.Test.FakeModels.EditFields;

namespace UnitTests.Requests
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

        [TestMethod]
        public void CreateEditFieldRequest()
        {
            var fields = new List<IFieldEditable>(new TextFieldFaker().Generate(5));
            var timestamp = new DateTime(2022, 1,1);

            var requestField = new EditFieldRequest(fields[0]) { ClientTime = timestamp };
            var requestFields = new EditFieldRequest(fields) { ClientTime = timestamp };

            Assert.AreEqual(1, requestField.Fields.Count);
            Assert.AreEqual(5, requestFields.Fields.Count);
            Assert.AreEqual(timestamp, requestField.ClientTime);
            Assert.AreEqual(timestamp, requestFields.ClientTime);
        }
    }
}
