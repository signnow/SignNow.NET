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
    /// <summary>
    /// This Test class contains all tests for Code Samples.
    /// </summary>
    [TestClass]
    public class ExamplesRunner
    {
        /// <summary>
        /// Base path to the `TestExamples` directory.
        /// Path should use Unix-like directory separator char. It requires for cross platform path compatibility.
        /// </summary>
        private static readonly string BaseTestExamplesPath = "../../../TestExamples/"
            .Replace('/', Path.DirectorySeparatorChar);

        private static readonly string PdfWithSignatureField = Path.Combine(BaseTestExamplesPath, "DocumentWithSignatureFieldTag.pdf");
        private static readonly string PdfWithoutFields = Path.Combine(BaseTestExamplesPath, "SignAndDate.pdf");

        /// <summary>Contains application clientId and clientSecret</summary>
        private static readonly CredentialModel ClientInfo = new CredentialLoader(ApiBaseUrl).GetCredentials();
        /// <summary>Contains user Email and Password</summary>
        private static readonly CredentialModel UserCredentials = new CredentialLoader(ApplicationBaseUrl).GetCredentials();

        /// <summary>Token for ExampleRunner</summary>
        private readonly Token token;

        /// <summary>
        /// SignNow service container used for ExampleRunner
        /// </summary>
        private readonly SignNowContext testContext;

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
            // Token for test runner
            token = AuthenticationExamples.RequestAccessToken(ApiBaseUrl, ClientInfo, UserCredentials).Result;
            testContext = new SignNowContext(ApiBaseUrl, token);
        }

        /// <summary>
        /// Delete test document after each test.
        /// </summary>
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

        /// <summary>
        /// Run test for example: <see cref="AuthenticationExamples.RequestAccessToken"/>
        /// </summary>
        [TestMethod]
        public void RequestAccessTokenTest()
        {
            var requestAccessToken = AuthenticationExamples.RequestAccessToken(ApiBaseUrl, ClientInfo, UserCredentials).Result;

            Assert.IsNotNull(requestAccessToken);
            Assert.IsFalse(string.IsNullOrEmpty(requestAccessToken.AccessToken));
            Assert.IsFalse(string.IsNullOrEmpty(requestAccessToken.RefreshToken));
        }

        #endregion

        #region Documents Examples

        /// <summary>
        /// Run test for example: <see cref="DocumentExamples.UploadDocumentWithFieldExtract"/>
        /// </summary>
        [TestMethod]
        public void UploadDocumentWithFieldExtractTest()
        {
            var documentWithFields = DocumentExamples
                .UploadDocumentWithFieldExtract(PdfWithSignatureField, token).Result;

            disposableDocumentId = documentWithFields?.Id;

            using var documentFields = documentWithFields.Fields.GetEnumerator();
            documentFields.MoveNext();

            Assert.AreEqual(FieldType.Text, documentFields.Current.Type);
            Assert.IsTrue(documentWithFields.Fields.Count > 0);
        }

        /// <summary>
        /// Run test for example: <see cref="DocumentExamples.DownloadSignedDocument"/>
        /// </summary>
        [TestMethod]
        public void DownloadSignedDocumentTest()
        {
            using var fileStream = File.OpenRead(PdfWithoutFields);
            var document = testContext.Documents
                .UploadDocumentAsync(fileStream, "SignedDocumentTest.pdf").Result;

            disposableDocumentId = document.Id;

            var documentSigned = DocumentExamples
                .DownloadSignedDocument(document.Id, token).Result;

            Assert.AreEqual("SignedDocumentTest.pdf", documentSigned.Filename);
            Assert.IsInstanceOfType(documentSigned.Document, typeof(Stream));
        }

        /// <summary>
        /// Run test for example: <see cref="DocumentExamples.CreateSigningLinkToTheDocument"/>
        /// </summary>
        [TestMethod]
        public void CreateSigningLintToTheDocumentTest()
        {
            using var fileStream = File.OpenRead(PdfWithSignatureField);
            var document = testContext.Documents
                .UploadDocumentWithFieldExtractAsync(fileStream, "CreateSigningLinkToTheDocument.pdf").Result;

            disposableDocumentId = document?.Id;

            var signingLink = DocumentExamples
                .CreateSigningLinkToTheDocument(document.Id, token).Result;

            Assert.IsNotNull(signingLink.Url);
            Assert.IsNotNull(signingLink.AnonymousUrl);
            Assert.IsInstanceOfType(signingLink.Url, typeof(Uri));
            Assert.AreEqual("https", signingLink.Url.Scheme);
            Assert.IsFalse(string.IsNullOrEmpty(signingLink.Url.OriginalString));
        }

        /// <summary>
        /// Run test for example: <see cref="DocumentExamples.CheckTheStatusOfTheDocument"/>
        /// </summary>
        [TestMethod]
        public void CheckTheStatusOfTheDocumentTest()
        {
            using var fileStream = File.OpenRead(PdfWithSignatureField);
            var document = testContext.Documents
                .UploadDocumentWithFieldExtractAsync(fileStream, "CheckTheStatusOfTheDocument.pdf").Result;

            disposableDocumentId = document?.Id;

            var documentStatus = DocumentExamples.CheckTheStatusOfTheDocument(document.Id, token).Result;

            Assert.AreEqual(DocumentStatus.NoInvite, documentStatus);
        }

        #endregion
    }
}
