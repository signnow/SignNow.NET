using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net.Examples.Authentication;
using SignNow.Net.Examples.Documents;
using SignNow.Net.Model;
using SignNow.Net.Test.Context;

namespace SignNow.Net.Examples
{
    [TestClass]
    public class ExamplesRunner
    {
        private readonly string baseTestExamplesPath = "../../../TestExamples/".Replace('/', Path.DirectorySeparatorChar);

        private static CredentialModel _clientInfo, _userCredentials;

        /// <summary>Token for ExampleRunner</summary>
        private readonly Token token;

        /// <summary>
        /// SignNow service container used for ExampleRunner
        /// </summary>
        private SignNowContext testContext;

        /// <summary>
        /// Document Id which should be deleted after each test
        /// </summary>
        private string disposableDocumentId;

        /// <summary>
        /// SignNow API base Url (sandbox)
        /// </summary>
        private static Uri ApiBaseUrl => new Uri("https://api-eval.signnow.com/");

        /// <summary>
        /// SignNow Application base Url (sandbox)
        /// </summary>
        private static Uri ApplicationBaseUrl => new Uri("https://app-eval.signnow.com/");

        public ExamplesRunner()
        {
            // Contains application clientId and clientSecret
            _clientInfo = new CredentialLoader(ApiBaseUrl).GetCredentials();
            // Contains user Email and Password
            _userCredentials = new CredentialLoader(ApplicationBaseUrl).GetCredentials();
            // Token for test runner
            token = AuthenticationExamples.RequestAccessToken(ApiBaseUrl, _clientInfo, _userCredentials).Result;
            testContext = new SignNowContext(ApiBaseUrl, token);
        }

        [TestCleanup]
        public void TearDown()
        {
            if (string.IsNullOrEmpty(disposableDocumentId))
            {
                return;
            }

            var documentTask = testContext.Documents
                .DeleteDocumentAsync(disposableDocumentId);

            Task.WaitAll(documentTask);

            Assert.IsFalse(documentTask.IsFaulted);

            disposableDocumentId = string.Empty;
        }

        #region Authentication Examples

        [TestMethod]
        public void RequestAccessTokenTest()
        {
            var requestAccessToken = AuthenticationExamples.RequestAccessToken(ApiBaseUrl, _clientInfo, _userCredentials).Result;

            Assert.IsNotNull(requestAccessToken);
            Assert.IsFalse(string.IsNullOrEmpty(requestAccessToken.AccessToken));
            Assert.IsFalse(string.IsNullOrEmpty(requestAccessToken.RefreshToken));
        }

        #endregion

        #region Documents Examples

        [TestMethod]
        public void UploadDocumentWithFieldExtractTest()
        {
            var pdfWithTags = Path.Combine(baseTestExamplesPath, "DocumentWithSignatureFieldTag.pdf");

            var documentWithFields = DocumentExamples
                .UploadDocumentWithFieldExtract(pdfWithTags, token).Result;

            disposableDocumentId = documentWithFields?.Id;

            using var documentFields = documentWithFields.Fields.GetEnumerator();
            documentFields.MoveNext();

            Assert.AreEqual(FieldType.Text, documentFields.Current.Type);
            Assert.IsTrue(documentWithFields.Fields.Count > 0);
        }

        [TestMethod]
        public void DownloadSignedDocument()
        {
            var testDocument = Path.Combine(baseTestExamplesPath, "DocumentWithSignatureFieldTag.pdf");

            using var fileStream = File.OpenRead(testDocument);
            var document = testContext.Documents
                .UploadDocumentAsync(fileStream, "SignedDocumentTest.pdf").Result;

            disposableDocumentId = document.Id;

            var documentSigned = DocumentExamples
                .DownloadSignedDocument(document.Id, token).Result;

            Assert.AreEqual("SignedDocumentTest.pdf", documentSigned.Filename);
            Assert.IsInstanceOfType(documentSigned.Document, typeof(Stream));
        }

        #endregion
    }
}
