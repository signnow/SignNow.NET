using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net.Interfaces;
using SignNow.Net.Service;
using SignNow.Net.Test;
using System.IO;
using System.Net.Http;

namespace AcceptanceTests
{
    [TestClass]
    public partial class DocumentServiceTest : AuthorizedApiTestBase
    {
        readonly IDocumentService docService;

        public string DocumentId { get; set; }

        /// <summary>
        /// Platfom specific Directory Separator Char
        /// </summary>
        readonly static char DS = Path.DirectorySeparatorChar;

        readonly string testDocumentsPath = $"..{DS}..{DS}..{DS}TestData{DS}Documents";
        readonly string pdfFileName = "DocumentUpload.pdf";
        readonly string txtFileName = "DocumentUpload.txt";

        private string PdfFilePath => Path.Combine(testDocumentsPath, pdfFileName);
        private string TxtFilePath => Path.Combine(testDocumentsPath, txtFileName);

        [TestCleanup]
        public void TestCleanup()
        {
            DeleteDocument(DocumentId);
        }

        public DocumentServiceTest()
        {
            docService = new DocumentService(ApiBaseUrl, Token);
        }

        /// <summary>
        /// Uploads Document from test fixtures.
        /// </summary>
        /// <param name="filePath">Path to test data.</param>
        /// <returns></returns>
        private string UploadTestDocument(string filePath)
        {
            string docId = default;

            using (var fileStream = File.OpenRead(filePath))
            {
                var uploadResponse = docService.UploadDocumentAsync(fileStream, pdfFileName).Result;
                docId = uploadResponse.Id;

                Assert.IsNotNull(
                    uploadResponse.Id,
                    "Document Upload result should contain non-null Id property value on successful upload"
                    );
            }

            return docId;
        }

        /// <summary>
        /// Cleanup Document uploaded by Unit tests.
        /// </summary>
        /// <param name="id">Identity of the document to be removed.</param>
        void DeleteDocument(string id)
        {
            if (string.IsNullOrEmpty(id))
                return;

            using (var client = new HttpClient())
            using (var requestMessage = new HttpRequestMessage(HttpMethod.Delete, $"{ApiBaseUrl}/document/{id}"))
            {
                requestMessage.Headers.Add("Authorization", Token.GetAuthorizationHeaderValue());
                var response = client.SendAsync(requestMessage).Result;
                response.EnsureSuccessStatusCode();
            }
        }
    }
}
