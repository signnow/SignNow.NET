using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net.Model.Responses;

namespace UnitTests.Responses
{
    [TestClass]
    public class DocumentGroupsResponseTests
    {
        [TestMethod]
        public void ShouldProperDeserialize()
        {
            var jsonResponse = @"{
            ""document_groups"": [
                {
                    ""folder_id"": null,
                    ""last_updated"": ""1729535107"",
                    ""group_id"": ""03c74b3083f34ebf8ef40a3039dfb32c85a08333"",
                    ""group_name"": ""CreateDocumentGroupTest"",
                    ""invite_id"": null,
                    ""invite_status"": null,
                    ""documents"": [
                    {
                        ""id"": ""66974a4b421546a69167ba342d1ae94af56ce333"",
                        ""name"": ""ForDocumentGroupFile-1"",
                        ""page_count"": 1,
                        ""thumbnail"": {
                            ""small"": ""https://api-eval.signnow.com/document/66974a4b421546a69167ba342d1ae94af56ce333/thumbnail?size=small"",
                            ""medium"": ""https://api-eval.signnow.com/document/66974a4b421546a69167ba342d1ae94af56ce333/thumbnail?size=medium"",
                            ""large"": ""https://api-eval.signnow.com/document/66974a4b421546a69167ba342d1ae94af56ce333/thumbnail?size=large""
                        },
                        ""roles"": [],
                        ""settings"": {
                            ""advanced_signing_flow"": false
                        },
                        ""has_credit_card_number"": false
                    }
                    ],
                    ""is_full_declined"": false,
                    ""is_embedded"": false,
                    ""freeform_invite"": {
                        ""id"": null
                    },
                    ""state"": ""created"",
                    ""has_guest_signer"": false,
                    ""has_signing_group"": false
                }
                ],
                ""document_group_total_count"": 1
            }";

            var response = TestUtils.DeserializeFromJson<DocumentGroupsResponse>(jsonResponse);

            Assert.AreEqual(1, response.TotalCount);
            Assert.AreEqual(1, response.Data.Count);

            var group = response.Data.First();
            Assert.AreEqual("03c74b3083f34ebf8ef40a3039dfb32c85a08333", group.GroupId);
            Assert.AreEqual("CreateDocumentGroupTest", group.Name);
            Assert.AreEqual(1, group.Documents.First().Pages);
            Assert.AreEqual("advanced_signing_flow", group.Documents.First().Settings.First().Key);
            Assert.AreEqual(false, group.Documents.First().Settings.First().Value);
        }
    }
}
