using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
using SignNow.Net.Internal.Helpers;
using SignNow.Net.Model;
using SignNow.Net.Model.Responses;
using SignNow.Net.Service;
using SignNow.Net.Test.FakeModels;
using UnitTests;

namespace UnitTests
{
    [TestClass]
    public class DocumentServiceBulkInviteTest : SignNowTestBase
    {
        [TestMethod]
        public async Task CreateBulkInviteFromTemplateAsync_Success()
        {
            // Arrange
            var templateId = "a1b2c3d4e5f6g7h8i9j0k1l2m3n4o5p6q7r8s9t0";
            var csvContent = "Signer 1|signer1@email.com,document_name\nSigner 1|signer2@email.com,document_name";
            var csvStream = new MemoryStream(Encoding.UTF8.GetBytes(csvContent));
            var fileName = "bulk_invite.csv";
            var folderId = "a1b2c3d4e5f6g7h8i9j0k1l2m3n4o5p6q7r8s9t0";
            var subject = "Please sign this document";
            var emailMessage = "Custom message for the signer";
            var clientTimestamp = 1640995200;
            var signatureType = "eideasy";

            var expectedResponse = new BulkInviteTemplateResponseFaker().Generate();
            var mockClient = SignNowClientMock(JsonConvert.SerializeObject(expectedResponse));

            var documentService = new DocumentService(ApiBaseUrl, new Token(), mockClient);

            // Act
            var result = await documentService.CreateBulkInviteFromTemplateAsync(
                templateId,
                csvStream,
                fileName,
                folderId,
                subject,
                emailMessage,
                clientTimestamp,
                signatureType,
                CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedResponse.Status, result.Status);
        }

        [TestMethod]
        public async Task CreateBulkInviteFromTemplateAsync_WithMinimalParameters_Success()
        {
            // Arrange
            var templateId = "a1b2c3d4e5f6g7h8i9j0k1l2m3n4o5p6q7r8s9t0";
            var csvContent = "Signer 1|signer1@email.com,document_name";
            var csvStream = new MemoryStream(Encoding.UTF8.GetBytes(csvContent));
            var fileName = "bulk_invite.csv";
            var folderId = "a1b2c3d4e5f6g7h8i9j0k1l2m3n4o5p6q7r8s9t0";

            var expectedResponse = new BulkInviteTemplateResponseFaker().Generate();
            var mockClient = SignNowClientMock(JsonConvert.SerializeObject(expectedResponse));

            var documentService = new DocumentService(ApiBaseUrl, new Token(), mockClient);

            // Act
            var result = await documentService.CreateBulkInviteFromTemplateAsync(
                templateId,
                csvStream,
                fileName,
                folderId,
                cancellationToken: CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedResponse.Status, result.Status);
        }

        [TestMethod]
        public async Task CreateBulkInviteFromTemplateAsync_WithCustomSubjectAndMessage_Success()
        {
            // Arrange
            var templateId = "a1b2c3d4e5f6g7h8i9j0k1l2m3n4o5p6q7r8s9t0";
            var csvContent = "Signer 1|signer1@email.com,document_name";
            var csvStream = new MemoryStream(Encoding.UTF8.GetBytes(csvContent));
            var fileName = "bulk_invite.csv";
            var folderId = "a1b2c3d4e5f6g7h8i9j0k1l2m3n4o5p6q7r8s9t0";
            var subject = "Please sign this document";
            var emailMessage = "Custom message for the signer";

            var expectedResponse = new BulkInviteTemplateResponseFaker().Generate();
            var mockClient = SignNowClientMock(JsonConvert.SerializeObject(expectedResponse));

            var documentService = new DocumentService(ApiBaseUrl, new Token(), mockClient);

            // Act
            var result = await documentService.CreateBulkInviteFromTemplateAsync(
                templateId,
                csvStream,
                fileName,
                folderId,
                subject,
                emailMessage,
                cancellationToken: CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedResponse.Status, result.Status);
        }

        [TestMethod]
        public async Task CreateBulkInviteFromTemplateAsync_WithClientTimestamp_Success()
        {
            // Arrange
            var templateId = "a1b2c3d4e5f6g7h8i9j0k1l2m3n4o5p6q7r8s9t0";
            var csvContent = "Signer 1|signer1@email.com,document_name";
            var csvStream = new MemoryStream(Encoding.UTF8.GetBytes(csvContent));
            var fileName = "bulk_invite.csv";
            var folderId = "a1b2c3d4e5f6g7h8i9j0k1l2m3n4o5p6q7r8s9t0";
            var clientTimestamp = (int)DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            var expectedResponse = new BulkInviteTemplateResponseFaker().Generate();
            var mockClient = SignNowClientMock(JsonConvert.SerializeObject(expectedResponse));

            var documentService = new DocumentService(ApiBaseUrl, new Token(), mockClient);

            // Act
            var result = await documentService.CreateBulkInviteFromTemplateAsync(
                templateId,
                csvStream,
                fileName,
                folderId,
                clientTimestamp: clientTimestamp,
                cancellationToken: CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedResponse.Status, result.Status);
        }
    }
}