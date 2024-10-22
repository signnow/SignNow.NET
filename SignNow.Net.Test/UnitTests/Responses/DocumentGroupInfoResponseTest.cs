using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net.Model.Responses;

namespace UnitTests.Responses
{
    [TestClass]
    public class DocumentGroupInfoResponseTest
    {
        [TestMethod]
        public void ShouldProperDeserialize()
        {
            var jsonResponse = @"{
            ""data"": {
                ""id"": ""03c74b3083f34ebf8ef40a3039dfb32c85a08437"",
                ""name"": ""CreateDocumentGroupTest"",
                ""created"": 1729535107,
                ""updated"": 1729535107,
                ""invite_id"": null,
                ""pending_step_id"": null,
                ""state"": ""created"",
                ""sign_as_merged"": null,
                ""last_invite_id"": null,
                ""owner_email"": ""signnow@gmail.com"",
                ""folder_id"": ""e1d8d63ba51c4009ab9941f279c908a0fd5a5e48"",
                ""documents"": [
                    {
                        ""roles"": [],
                        ""document_name"": ""ForDocumentGroupFile-1"",
                        ""page_count"": 1,
                        ""id"": ""66974a4b421546a69167ba342d1ae94af56ce351"",
                        ""updated"": 1729535105,
                        ""folder_id"": ""e1d8d63ba51c4009ab9941f279c908a0fd5a5e48"",
                        ""owner"": {
                            ""id"": ""50204b3344984768bb16d61f8550f8b5edfd719a"",
                            ""email"": ""signnow@gmail.com""
                        },
                        ""thumbnail"": {
                            ""small"": ""https://api-eval.signnow.com/document/66974a4b421546a69167ba342d1ae94af56ce351/thumbnail?size=small"",
                            ""medium"": ""https://api-eval.signnow.com/document/66974a4b421546a69167ba342d1ae94af56ce351/thumbnail?size=medium"",
                            ""large"": ""https://api-eval.signnow.com/document/66974a4b421546a69167ba342d1ae94af56ce351/thumbnail?size=large""
                        },
                        ""origin_document_id"": null,
                        ""has_unassigned_field"": false,
                        ""has_credit_card_number"": false,
                        ""field_invites"": [],
                        ""shared_with_team"": null,
                        ""settings"": [],
                        ""allow_to_remove"": true
                    }
                ],
                ""owner"": {
                    ""id"": ""50204b3344984768bb16d61f8550f8b5edfd719a"",
                    ""email"": ""signnow@gmail.com"",
                    ""organization"": {
                        ""id"": ""44f2dc3eef1c4924a8aea48d238968402abc745f""
                    }
                },
                ""cc_emails"": [],
                ""freeform_invite"": {
                    ""id"": null,
                    ""last_id"": null
                },
                ""originator_organization_settings"": [
                    {
                        ""setting"": ""mobileweb_option"",
                        ""value"": ""app_or_mobileweb_choice""
                    }
                ],
                ""mail_provider"": null
                }
            }";

            var response = TestUtils.DeserializeFromJson<DocumentGroupInfoResponse>(jsonResponse);

            Assert.AreEqual("03c74b3083f34ebf8ef40a3039dfb32c85a08437", response.Data.Id);
            Assert.AreEqual("CreateDocumentGroupTest", response.Data.Name);
            Assert.AreEqual(1, response.Data.Documents.Count);
            Assert.AreEqual("ForDocumentGroupFile-1", response.Data.Documents.FirstOrDefault()?.Name);
            Assert.AreEqual("50204b3344984768bb16d61f8550f8b5edfd719a", response.Data.Owner.Id);
            Assert.AreEqual("44f2dc3eef1c4924a8aea48d238968402abc745f", response.Data.Owner.Organization.Id);
        }
    }
}
