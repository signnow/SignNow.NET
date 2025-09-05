using System.IO;
using System.Net.Http;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net.Internal.Requests;

namespace UnitTests
{
    [TestClass]
    public class BulkInviteTemplateRequestTest : SignNowTestBase
    {
        [TestMethod]
        public void CanGetHttpContentWithRequiredParameters()
        {
            // Arrange
            var csvContent = "Signer 1|signer1@email.com,document_name\nSigner 1|signer2@email.com,document_name";
            var csvStream = new MemoryStream(Encoding.UTF8.GetBytes(csvContent));
            var fileName = "bulk_invite.csv";
            var folderId = "a1b2c3d4e5f6g7h8i9j0k1l2m3n4o5p6q7r8s9t0";

            var request = new BulkInviteTemplateRequest(csvStream, fileName, folderId);

            // Act
            var httpContent = request.GetHttpContent();

            // Assert
            var requestContent = httpContent.ReadAsStringAsync().Result;
            
            StringAssert.StartsWith(httpContent.Headers.ContentType.ToString(), "multipart/form-data");
            StringAssert.Contains(
                requestContent,
                "Content-Disposition: form-data; name=file; filename=bulk_invite.csv; filename*=utf-8''bulk_invite.csv");
            StringAssert.Contains(
                requestContent,
                "Content-Disposition: form-data; name=folder_id");
            StringAssert.Contains(requestContent, folderId);
            
            // Verify optional parameters that were not provided are not included
            Assert.IsFalse(requestContent.Contains("email_message"));
            Assert.IsFalse(requestContent.Contains("client_timestamp"));
            Assert.IsFalse(requestContent.Contains("signature_type"));
        }

        [TestMethod]
        public void CanGetHttpContentWithAllParameters()
        {
            // Arrange
            var csvContent = "Signer 1|signer1@email.com,document_name\nSigner 1|signer2@email.com,document_name";
            var csvStream = new MemoryStream(Encoding.UTF8.GetBytes(csvContent));
            var fileName = "bulk_invite.csv";
            var folderId = "a1b2c3d4e5f6g7h8i9j0k1l2m3n4o5p6q7r8s9t0";
            var subject = "Please sign this document";
            var emailMessage = "Custom message for the signer";
            var clientTimestamp = 1640995200;
            var signatureType = "eideasy";

            var request = new BulkInviteTemplateRequest(
                csvStream, 
                fileName, 
                folderId, 
                subject, 
                emailMessage, 
                clientTimestamp, 
                signatureType);

            // Act
            var httpContent = request.GetHttpContent();

            // Assert
            var requestContent = httpContent.ReadAsStringAsync().Result;
            
            StringAssert.StartsWith(httpContent.Headers.ContentType.ToString(), "multipart/form-data");
            StringAssert.Contains(
                requestContent,
                "Content-Disposition: form-data; name=file; filename=bulk_invite.csv; filename*=utf-8''bulk_invite.csv");
            StringAssert.Contains(
                requestContent,
                "Content-Disposition: form-data; name=folder_id");
            StringAssert.Contains(requestContent, folderId);
            StringAssert.Contains(
                requestContent,
                "Content-Disposition: form-data; name=subject");
            StringAssert.Contains(requestContent, subject);
            StringAssert.Contains(
                requestContent,
                "Content-Disposition: form-data; name=email_message");
            StringAssert.Contains(requestContent, emailMessage);
            StringAssert.Contains(
                requestContent,
                "Content-Disposition: form-data; name=client_timestamp");
            StringAssert.Contains(requestContent, clientTimestamp.ToString());
            StringAssert.Contains(
                requestContent,
                "Content-Disposition: form-data; name=signature_type");
            StringAssert.Contains(requestContent, signatureType);
        }
    }
}