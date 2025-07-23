using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net.Internal.Extensions;
using SignNow.Net.Model;
using SignNow.Net.Test.FakeModels;

namespace UnitTests.Responses
{
    [TestClass]
    public class DocumentHistoryResponseTest
    {
        [TestMethod]
        public void ShouldProperDeserialize()
        {
            var historyResponse = new DocumentHistoryResponseFaker().Generate(5);
            var historyJson = TestUtils.SerializeToJsonFormatted(historyResponse);

            var response = TestUtils.DeserializeFromJson<List<DocumentHistoryResponse>>(historyJson);

            Assert.AreEqual(5, response.Count);
            response.ForEach(itm => Assert.IsNotNull(itm.Id.ValidateId()));
            response.ForEach(itm => Assert.IsNotNull(itm.DocumentId.ValidateId()));
            response.ForEach(itm => Assert.IsInstanceOfType(itm.Created, typeof(DateTime)));
        }

        [TestMethod]
        public void ShouldDeserializeDynamicallyFilledJsonAttributes()
        {
            var asObject = @"[
                {
                    ""unique_id"": ""3b689c690000a1a8b70b353eb581c2f88f667c9b"",
                    ""created"": ""1564755881"",
                    ""client_timestamp"": null,
                    ""event"": ""created_document"",
                    ""client_app_name"": ""Test integration"",
                    ""ip_address"": ""127.0.0.1"",
                    ""user_id"": ""00008b4a21bc364deda9fe7777fa75a685e7a9e0"",
                    ""email"": ""signnow.user@signnow.com"",
                    ""document_id"": ""ff979458faedf6b632869e7cb3f0000341fb9ae6"",
                    ""field_id"": null,
                    ""element_id"": null,
                    ""origin"": ""original"",
                    ""json_attributes"": {
                        ""signers"": [""signer1@gmail.com""],
                        ""viewers"": [],
                        ""authentication_type"": ""password""
                    }
                }
            ]";
            var asArray = @"[
                {
                    ""unique_id"": ""3b689c690000a1a8b70b353eb581c2f88f667c9b"",
                    ""created"": ""1564755881"",
                    ""client_timestamp"": null,
                    ""event"": ""created_document"",
                    ""client_app_name"": ""Test integration"",
                    ""ip_address"": ""127.0.0.1"",
                    ""user_id"": ""00008b4a21bc364deda9fe7777fa75a685e7a9e0"",
                    ""email"": ""signnow.user@signnow.com"",
                    ""document_id"": ""ff979458faedf6b632869e7cb3f0000341fb9ae6"",
                    ""field_id"": null,
                    ""element_id"": null,
                    ""origin"": ""original"",
                    ""json_attributes"": []
                }
            ]";

            var responseObject = TestUtils.DeserializeFromJson<List<DocumentHistoryResponse>>(asObject);
            var responseArray = TestUtils.DeserializeFromJson<List<DocumentHistoryResponse>>(asArray);

            Assert.AreEqual("3b689c690000a1a8b70b353eb581c2f88f667c9b", responseObject[0].Id);
            Assert.AreEqual("3b689c690000a1a8b70b353eb581c2f88f667c9b", responseArray[0].Id);
            Assert.IsInstanceOfType(responseArray[0].JsonAttributes, typeof(Newtonsoft.Json.Linq.JArray));
            Assert.IsInstanceOfType(responseObject[0].JsonAttributes, typeof(Newtonsoft.Json.Linq.JObject));
        }
    }
}
