using System;
using System.IO;
using System.Net.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net;
using SignNow.Net.Model;
using SignNow.Net.Test.Context;

namespace UnitTests
{
    [TestClass]
    public class AuthorizedApiTestBase : SignNowTestBase
    {
        /// <summary>
        /// Token for all authorized API tests
        /// </summary>
        protected static Token Token { get; set; }

        /// <summary>
        /// Entry point for all signNow services by <see cref="SignNowContext"/>
        /// </summary>
        public static SignNowContext SignNowTestContext { get; set; }

        public static HttpClient Client { get; set; }

        /// <summary>
        /// Uploaded test document identity which will be deleted after run TestCase.
        /// </summary>
        protected string DisposableDocumentId { get; set; }

        /// <summary>
        /// Pdf document required for almost all Acceptance tests
        /// </summary>
        protected static string TestPdfDocumentId { get; set; }

        /// <summary>
        /// Pdf document with Fields required for almost all Acceptance tests
        /// </summary>
        protected static string TestPdfDocumentIdWithFields { get; set; }

        /// <summary>
        /// Use this method to upload all required for test documents
        /// </summary>
        /// <param name="context"></param>
        [AssemblyInitialize]
        public static void AssemblyInitialize(TestContext context)
        {
            var loader = new CredentialLoader(ApiBaseUrl);
            var apiCreds = loader.GetCredentials();

            Client = new HttpClient();

            SignNowTestContext = new SignNowContext(ApiBaseUrl, null, Client);
            SignNowTestContext.SetAppCredentials(apiCreds.ClientId, apiCreds.ClientSecret);
            SignNowTestContext.GetAccessToken(apiCreds.Login, apiCreds.Password, Scope.All);

            TestPdfDocumentId = UploadTestDocument(PdfFilePath);
            TestPdfDocumentIdWithFields = UploadTestDocumentWithFieldExtract(PdfFilePath);
        }

        [AssemblyCleanup]
        public static void AssemblyCleanup()
        {
            DeleteTestDocument(TestPdfDocumentId);
            DeleteTestDocument(TestPdfDocumentIdWithFields);
        }

        [TestCleanup]
        public void TearDown()
        {
            DeleteTestDocument(DisposableDocumentId);
        }

        /// <summary>
        /// Uploads Document from test fixtures.
        /// </summary>
        /// <param name="filePath">Path to test data.</param>
        /// <param name="fileName">Custom file name, if empty <see cref="SignNowTestBase.PdfFileName"/> will be used</param>
        /// <returns></returns>
        protected static string UploadTestDocument(string filePath, string fileName = "")
            => ProcessUploadDocument(filePath, fileName);

        /// <summary>
        /// Uploads Document with Fields Extract from test fixtures.
        /// </summary>
        /// <param name="filePath">Path to test data.</param>
        /// <param name="fileName">Custom file name, if empty <see cref="SignNowTestBase.PdfFileName"/> will be used</param>
        /// <returns></returns>
        protected static string UploadTestDocumentWithFieldExtract(string filePath, string fileName = "")
            => ProcessUploadDocument(filePath, fileName, true);

        private static string ProcessUploadDocument(string filePath, string fileName, bool extractFields = false)
        {
            string docId = default;

            if (String.IsNullOrEmpty(fileName))
            {
                fileName = PdfFileName;
            }

            using var fileStream = File.OpenRead(filePath);
            var uploadTask = extractFields
                ? SignNowTestContext.Documents.UploadDocumentWithFieldExtractAsync(fileStream, fileName)
                : SignNowTestContext.Documents.UploadDocumentAsync(fileStream, fileName);

            var uploadResponse = uploadTask.Result;
            docId = uploadResponse?.Id;

            Assert.IsNotNull(
                uploadResponse?.Id,
                "Document Upload result should contain non-null Id property value on successful upload"
            );

            return docId;
        }

        /// <summary>
        /// Cleanup Document uploaded by Unit tests.
        /// </summary>
        /// <param name="documentId">Identity of the document to be removed.</param>
        private static void DeleteTestDocument(string documentId)
        {
            if (string.IsNullOrEmpty(documentId))
            {
                return;
            }

            SignNowTestContext.Documents.DeleteDocumentAsync(documentId);
        }
    }
}
