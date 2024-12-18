using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net.Model;
using SignNow.Net.Test.Context;

namespace SignNow.Net.Examples
{
    /// <summary>
    /// This Test class contains all tests for Code Samples.
    ///
    /// To run single test from console:
    ///     # For example we want to run only RequestAccessTokenTest
    ///     dotnet test SignNow.Net.Examples --filter RequestAccessTokenTest
    /// </summary>
    [TestClass]
    public abstract class ExamplesRunner
    {
        protected DateTime UnixEpoch => new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        /// <summary>
        /// Base path to the `TestExamples` directory.
        /// Path should use Unix-like directory separator char. It requires for cross-platform path compatibility.
        /// </summary>
        private static readonly string BaseTestExamplesPath = "../../../TestExamples/"
            .Replace('/', Path.DirectorySeparatorChar);

        protected static readonly string PdfWithSignatureField = Path.Combine(BaseTestExamplesPath, "DocumentWithSignatureFieldTag.pdf");
        protected static readonly string PdfWithoutFields = Path.Combine(BaseTestExamplesPath, "SignAndDate.pdf");
        protected static readonly string PdfWithComplexTags = Path.Combine(BaseTestExamplesPath, "ComplexTags.pdf");

        /// <summary>
        /// Contains application clientId, clientSecret and user credentials
        /// </summary>
        protected static CredentialModel credentials = new CredentialLoader(ApiBaseUrl).GetCredentials();

        /// <summary>
        /// signNow service container used for ExampleRunner
        /// </summary>
        protected static SignNowContext testContext;

        /// <summary>
        /// signNow API base Url (sandbox)
        /// </summary>
        public static Uri ApiBaseUrl => new Uri("https://api-eval.signnow.com/");

        /// <summary>
        /// Delete test document after test.
        /// </summary>
        protected void DeleteTestDocument(string disposableDocumentId)
        {
            if (string.IsNullOrEmpty(disposableDocumentId))
            {
                return;
            }

            var documentTask = testContext.Documents
                .DeleteDocumentAsync(disposableDocumentId);

            Task.WaitAll(documentTask);

            Assert.IsFalse(documentTask.IsFaulted);
        }

        [AssemblyInitialize]
        public static void AssemblyInitialize(TestContext context)
        {
            // If you want to use your own credentials just for simple and fast test
            // uncomment next lines bellow and replace placeholders with your credentials:

            // credentials = new CredentialModel
            // {
            //     Login = "user_eamail@noemail.com",
            //     Password = "your-secret-password",
            //     ClientId = "your-application-client-id",
            //     ClientSecret = "your-application-client-secret"
            // };

            var client = new HttpClient();

            // Create signNow context with all the services and Authorization
            testContext = new SignNowContext(ApiBaseUrl, null, client);
            testContext.SetAppCredentials(credentials.ClientId, credentials.ClientSecret);
            testContext.GetAccessToken(credentials.Login, credentials.Password, Scope.All);
        }
    }
}
