using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net.Model.Responses;
using SignNow.Net.Test.TestData.FakeModels;
using System.Linq;
using UnitTests;

namespace SignNow.Net.Test.UnitTests.Responses
{
    [TestClass]
    public class GetDocumentGroupTemplatesResponseTest
    {
        [TestMethod]
        public void GetDocumentGroupTemplatesResponse_DeserializationTest()
        {
            var jsonResponse = @"{
                ""document_group_templates"": [
                    {
                        ""folder_id"": null,
                        ""last_updated"": ""1634828541"",
                        ""template_group_id"": ""31706abc6e50c977af03c1cacd3875a44fb679b9"",
                        ""template_group_name"": ""DGT test"",
                        ""owner_email"": ""test.user@example.com"",
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
                            ""invite_steps"": []
                        }
                    }
                ],
                ""document_group_template_total_count"": 1
            }";

            var response = TestUtils.DeserializeFromJson<GetDocumentGroupTemplatesResponse>(jsonResponse);

            Assert.IsNotNull(response);
            Assert.IsNotNull(response.DocumentGroupTemplates);
            Assert.AreEqual(1, response.DocumentGroupTemplates.Count);
            Assert.AreEqual(1, response.DocumentGroupTemplateTotalCount);

            var template = response.DocumentGroupTemplates.First();
            Assert.AreEqual("31706abc6e50c977af03c1cacd3875a44fb679b9", template.TemplateGroupId);
            Assert.AreEqual("DGT test", template.TemplateGroupName);
            Assert.AreEqual("test.user@example.com", template.OwnerEmail);
            Assert.IsFalse(template.IsPrepared);
            Assert.IsNotNull(template.Templates);
            Assert.AreEqual(1, template.Templates.Count);
            Assert.IsNotNull(template.RoutingDetails);
            Assert.IsTrue(template.RoutingDetails.SignAsMerged);

            var templateItem = template.Templates.First();
            Assert.AreEqual("a8f84795001f4add81dc8efc45d97fdeed9a00aa", templateItem.Id);
            Assert.AreEqual("Test_PDF 1 T", templateItem.Name);
            Assert.IsNotNull(templateItem.Thumbnail);
            Assert.IsNotNull(templateItem.Roles);
            Assert.AreEqual(2, templateItem.Roles.Count);
            Assert.IsTrue(templateItem.Roles.Contains("Signer 1"));
            Assert.IsTrue(templateItem.Roles.Contains("Signer 2"));
        }

        [TestMethod]
        public void GetDocumentGroupTemplatesResponse_FakerTest()
        {
            var response = new GetDocumentGroupTemplatesResponseFaker().Generate();

            Assert.IsNotNull(response);
            Assert.IsNotNull(response.DocumentGroupTemplates);
            Assert.IsTrue(response.DocumentGroupTemplateTotalCount > 0);
            Assert.IsTrue(response.DocumentGroupTemplates.Count > 0);

            var template = response.DocumentGroupTemplates.First();
            Assert.IsNotNull(template.TemplateGroupId);
            Assert.IsNotNull(template.TemplateGroupName);
            Assert.IsNotNull(template.OwnerEmail);
            Assert.IsNotNull(template.Templates);
            Assert.IsTrue(template.Templates.Count > 0);
        }
    }
}
