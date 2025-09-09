using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net.Model;
using SignNow.Net.Model.Requests.DocumentGroup;
using SignNow.Net.Model.Responses;
using SignNow.Net.Service;
using SignNow.Net.Test.TestData.FakeModels;
using System.Threading.Tasks;
using System.Linq;
using UnitTests;

namespace SignNow.Net.Test.UnitTests.Services
{
    [TestClass]
    public class DocumentGroupServiceGetTemplatesTest : SignNowTestBase
    {
        [TestMethod]
        public async Task GetDocumentGroupTemplatesAsyncTest()
        {
            var jsonResponse = @"{
                ""document_group_templates"": [
                    {
                        ""folder_id"": null,
                        ""last_updated"": ""1634828541"",
                        ""template_group_id"": ""31706abc6e50c977af03c1cacd3875a44fb679b9"",
                        ""template_group_name"": ""DGT test"",
                        ""owner_email"": ""kulygin.denys@pdffiller.team"",
                        ""templates"": [
                            {
                                ""id"": ""a8f84795001f4add81dc8efc45d97fdeed9a00aa"",
                                ""name"": ""Test_PDF 1 T"",
                                ""thumbnail"": {
                                    ""small"": ""https://app.signnow.com/api/document/a8f84795001f4add81dc8efc45d97fdeed9a00aa/thumbnail?size=small"",
                                    ""medium"": ""https://app.signnow.com/api/document/a8f84795001f4add81dc8efc45d97fdeed9a00aa/thumbnail?size=medium"",
                                    ""large"": ""https://app.signnow.com/api/document/a8f84795001f4add81dc8efc45d97fdeed9a00aa/thumbnail?size=large""
                                },
                                ""roles"": [""Signer 1"", ""Signer 2""]
                            }
                        ],
                        ""is_prepared"": false,
                        ""routing_details"": {
                            ""sign_as_merged"": true,
                            ""include_email_attachments"": null,
                            ""invite_steps"": [
                                {
                                    ""order"": 1,
                                    ""invite_emails"": [
                                        {
                                            ""email"": ""mail+1@gmail.com"",
                                            ""subject"": ""DGT test: Signature Request from kulygin.denys"",
                                            ""message"": ""kulygin.denys@pdffiller.team invited you to sign some documents."",
                                            ""reminder"": {
                                                ""remind_before"": 0,
                                                ""remind_after"": 0,
                                                ""remind_repeat"": 0
                                            },
                                            ""expiration_days"": 30,
                                            ""has_sign_actions"": true
                                        }
                                    ],
                                    ""invite_actions"": [
                                        {
                                            ""email"": ""mail+1@gmail.com"",
                                            ""authentication"": {
                                                ""type"": null
                                            },
                                            ""uuid"": ""46f82586-c16b-463d-8835-52ce5c552e08"",
                                            ""allow_reassign"": 0,
                                            ""decline_by_signature"": 0,
                                            ""action"": ""sign"",
                                            ""role_name"": ""Signer 1"",
                                            ""document_id"": ""5507722db6654b11bd2223b83655443813f2013c"",
                                            ""document_name"": ""Test_PDF 1 from templ 2""
                                        }
                                    ]
                                }
                            ]
                        }
                    }
                ],
                ""document_group_template_total_count"": 1
            }";

            var service = new DocumentGroupService(ApiBaseUrl, new Token(),
                SignNowClientMock(jsonResponse));

            var request = new GetDocumentGroupTemplatesRequestFaker().Generate();
            var response = await service.GetDocumentGroupTemplatesAsync(request).ConfigureAwait(false);

            Assert.IsInstanceOfType(response, typeof(GetDocumentGroupTemplatesResponse));
            Assert.IsNotNull(response.DocumentGroupTemplates);
            Assert.AreEqual(1, response.DocumentGroupTemplates.Count);
            Assert.AreEqual(1, response.DocumentGroupTemplateTotalCount);
            
            var template = response.DocumentGroupTemplates[0];
            Assert.AreEqual("31706abc6e50c977af03c1cacd3875a44fb679b9", template.TemplateGroupId);
            Assert.AreEqual("DGT test", template.TemplateGroupName);
            Assert.AreEqual("kulygin.denys@pdffiller.team", template.OwnerEmail);
            Assert.IsFalse(template.IsPrepared);
            Assert.IsNotNull(template.Templates);
            Assert.AreEqual(1, template.Templates.Count);
            Assert.IsNotNull(template.RoutingDetails);
            Assert.IsTrue(template.RoutingDetails.SignAsMerged);
        }

        [TestMethod]
        public async Task GetDocumentGroupTemplatesAsyncWithFakerTest()
        {
            var fakeResponse = new GetDocumentGroupTemplatesResponseFaker().Generate();
            var jsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(fakeResponse);
            
            var service = new DocumentGroupService(ApiBaseUrl, new Token(),
                SignNowClientMock(jsonResponse));

            var request = new GetDocumentGroupTemplatesRequestFaker().Generate();
            var response = await service.GetDocumentGroupTemplatesAsync(request).ConfigureAwait(false);

            Assert.IsInstanceOfType(response, typeof(GetDocumentGroupTemplatesResponse));
            Assert.IsNotNull(response.DocumentGroupTemplates);
            Assert.IsTrue(response.DocumentGroupTemplateTotalCount > 0);
        }
    }
}
