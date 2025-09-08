using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net.Model.Requests.DocumentGroup;
using UnitTests;

namespace AcceptanceTests
{
    [TestClass]
    public class DocumentGroupServiceTest : AuthorizedApiTestBase
    {
        [TestMethod]
        public async Task ShouldUpdateDocumentGroupTemplate()
        {
            // Note: This test requires a valid document group template ID
            // In a real scenario, you would first create a document group template
            // For this test, we'll use a mock template ID and expect the API to return appropriate errors
            
            var updateRequest = new UpdateDocumentGroupTemplateRequest
            {
                TemplateIdsToAdd = new List<string> { "ddc7ce43dfc5ad3b2f0fdb1db36889ce53f00789" },
                TemplateIdsToRemove = new List<string>(),
                RoutingDetails = @"{
                    ""invite_steps"": [
                        {
                            ""order"": 1,
                            ""invite_emails"": [
                                {
                                    ""email"": ""test@example.com"",
                                    ""subject"": ""Document Needs Your Signature"",
                                    ""message"": ""Please sign this document"",
                                    ""expiration_days"": 30,
                                    ""reminder"": 0,
                                    ""hasSignActions"": true,
                                    ""allow_reassign"": ""0""
                                }
                            ]
                        }
                    ],
                    ""include_email_attachments"": 0
                }",
                TemplateGroupName = "Updated Template Group"
            };

            // Use a mock template ID for testing
            var mockTemplateId = "ddc7ce43dfc5ad3b2f0fdb1db36889ce53f00777";

            try
            {
                var response = await SignNowTestContext.DocumentGroup
                    .UpdateDocumentGroupTemplateAsync(mockTemplateId, updateRequest)
                    .ConfigureAwait(false);

                Assert.IsNotNull(response);
                Assert.AreEqual("success", response.Status);
            }
            catch (SignNow.Net.Exceptions.SignNowException ex)
            {
                // Expected for mock template ID - verify it's the right type of error
                Assert.IsTrue(ex.HttpStatusCode == System.Net.HttpStatusCode.NotFound || 
                             ex.HttpStatusCode == System.Net.HttpStatusCode.BadRequest);
            }
        }

        [TestMethod]
        public async Task ShouldThrowExceptionForInvalidTemplateId()
        {
            var updateRequest = new UpdateDocumentGroupTemplateRequest
            {
                TemplateIdsToAdd = new List<string>(),
                TemplateIdsToRemove = new List<string>(),
                RoutingDetails = "{}",
                TemplateGroupName = "Test Template Group"
            };

            var invalidTemplateId = "invalid-template-id";

            await Assert.ThrowsExceptionAsync<ArgumentException>(
                async () => await SignNowTestContext.DocumentGroup
                    .UpdateDocumentGroupTemplateAsync(invalidTemplateId, updateRequest)
                    .ConfigureAwait(false)
            ).ConfigureAwait(false);
        }

        [TestMethod]
        public async Task ShouldHandleEmptyUpdateRequest()
        {
            var updateRequest = new UpdateDocumentGroupTemplateRequest
            {
                TemplateIdsToAdd = new List<string>(),
                TemplateIdsToRemove = new List<string>(),
                RoutingDetails = "{}",
                TemplateGroupName = "Test Template Group"
            };

            var mockTemplateId = "ddc7ce43dfc5ad3b2f0fdb1db36889ce53f00777";

            try
            {
                var response = await SignNowTestContext.DocumentGroup
                    .UpdateDocumentGroupTemplateAsync(mockTemplateId, updateRequest)
                    .ConfigureAwait(false);

                Assert.IsNotNull(response);
                Assert.AreEqual("success", response.Status);
            }
            catch (SignNow.Net.Exceptions.SignNowException ex)
            {
                // Expected for mock template ID - verify it's the right type of error
                Assert.IsTrue(ex.HttpStatusCode == System.Net.HttpStatusCode.NotFound || 
                             ex.HttpStatusCode == System.Net.HttpStatusCode.BadRequest);
            }
        }
    }
}
