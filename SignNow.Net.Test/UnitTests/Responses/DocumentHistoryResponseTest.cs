using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net.Model;

namespace UnitTests.Responses
{
    [TestClass]
    public class DocumentHistoryResponseTest
    {
        [TestMethod]
        public void ShouldProperDeserialize()
        {
            var jsonResponse = @"[
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
                    ""json_attributes"": null,
                    ""origin"": ""original""
                }
            ]";

            var response = TestUtils.DeserializeFromJson<List<DocumentHistoryResponse>>(jsonResponse);

            Assert.AreEqual("3b689c690000a1a8b70b353eb581c2f88f667c9b", response[0].Id);
        }
    }
}
