using System.IO;
using System.Net.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net.Interfaces;
using SignNow.Net.Model;
using SignNow.Net.Test.Context;

namespace SignNow.Net.Test
{
    [TestClass]
    public class AuthorizedApiTestBase : ApiTestBase
    {
        protected Token Token { get; private set; }

        /// <summary>
        /// Uploaded test document identity which will be deleted after run TestCase.
        /// </summary>
        protected string DocumentId { get; set; }

        [TestCleanup]
        public void TearDown()
        {
            DeleteDocument(DocumentId);
        }

        protected AuthorizedApiTestBase()
        {
            var userCredentialsLoader = new CredentialLoader(ApplicationBaseUrl);
            var apiCredentialsLoader = new CredentialLoader(ApiBaseUrl);
            var apiCreds = apiCredentialsLoader.GetCredentials();
            var userCreds = userCredentialsLoader.GetCredentials();
            var oauth = new OAuth2Service(ApiBaseUrl, apiCreds.Login, apiCreds.Password);

            Token = oauth.GetTokenAsync(userCreds.Login, userCreds.Password, Scope.All).Result;
        }

        /// <summary>
        /// Uploads Document from test fixtures.
        /// </summary>
        /// <param name="filePath">Path to test data.</param>
        /// <param name="docService"></param>
        /// <returns></returns>
        protected string UploadTestDocument(string filePath, IDocumentService docService)
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
        protected void DeleteDocument(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return;
            }

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
