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


        // #region WebHooks Examples
        //
        // /// <summary>
        // /// Allows to subscribe an external service(callback_url) to a specific event of user or document.
        // /// As soon as a certain selected event from the List of event types occurs, SignNow sends a notification about it.
        // /// </summary>
        // /// <see cref="https://docs.signnow.com/docs/signnow/branches/v1.2/reference/operations/create-a-api-v-2-event"/>
        // [TestMethod]
        // public async Task CreateEventSubscription()
        // {
        //     // Upload document with fields
        //     await using var fileStream = File.OpenRead(PdfWithSignatureField);
        //     var document = await testContext.Documents
        //         .UploadDocumentWithFieldExtractAsync(fileStream, "DocumentForEventSubscriptionCreate.pdf")
        //         .ConfigureAwait(false);
        //
        //     // Using signNowContext lets create event subscription
        //     var myCallbackUrl = new Uri("https://signnow.com/callbackHandler");
        //     await testContext.Events
        //         .CreateEventSubscriptionAsync(
        //             new CreateEventSubscription(EventType.DocumentComplete, document.Id, myCallbackUrl))
        //         .ConfigureAwait(false);
        //
        //     // Check for successful created event subscription
        //     // Gets information about all subscriptions to events made with a specific application
        //     // <see cref="https://docs.signnow.com/docs/signnow/reference/operations/list-api-v-2-events"/>
        //     var eventSubscriptionList = await testContext.Events
        //         .GetEventSubscriptionsAsync(new PagePaginationOptions { Page = 1, PerPage = 1})
        //         .ConfigureAwait(false);
        //
        //     // Determining events total to get the latest event from whole events list
        //     var latestPage = eventSubscriptionList.Meta.Pagination.Total;
        //
        //     // Getting event details from Events list
        //     var myLatestCreatedEvent = await testContext.Events
        //         .GetEventSubscriptionsAsync(new PagePaginationOptions { Page = latestPage, PerPage = 1})
        //         .ConfigureAwait(false);
        //
        //     var myLatestEvent = myLatestCreatedEvent.Data.First();
        //     Assert.AreEqual(EventType.DocumentComplete, myLatestEvent.Event);
        //     Assert.AreEqual(myCallbackUrl, myLatestEvent.JsonAttributes.CallbackUrl);
        //
        //     // Changing an existing event subscription
        //     // <see cref="https://docs.signnow.com/docs/signnow/reference/operations/update-a-api-v-2-event"/>
        //     var eventForUpdate = myLatestCreatedEvent.Data.First();
        //     eventForUpdate.JsonAttributes.CallbackUrl = new Uri("https://signnow.com/myNewCallbackHandler");
        //
        //     var changedEvent = await testContext.Events
        //         .UpdateEventSubscriptionAsync(new UpdateEventSubscription(eventForUpdate))
        //         .ConfigureAwait(false);
        //
        //     var updatedEvent = await testContext.Events
        //         .GetEventSubscriptionInfoAsync(changedEvent.Id)
        //         .ConfigureAwait(false);
        //
        //     Assert.AreEqual(myLatestEvent.Id, updatedEvent.Id);
        //     Assert.AreEqual("https://signnow.com/myNewCallbackHandler", updatedEvent.JsonAttributes.CallbackUrl.AbsoluteUri);
        //
        //     // Unsubscribes an external service (callback_url) from specific events of user or document
        //     // <see cref="https://docs.signnow.com/docs/signnow/reference/operations/delete-a-api-v-2-event"/>
        //     await testContext.Events
        //         .DeleteEventSubscriptionAsync(myLatestEvent.Id)
        //         .ConfigureAwait(false);
        // }
        //
        // #endregion
        //
        // #region DocumentGroup Examples
        //
        // [TestMethod]
        // public async Task CreateDocumentGroup()
        // {
        //     // Upload test documents
        //     await using var fileStream = File.OpenRead(PdfWithSignatureField);
        //
        //     var documents = new List<SignNowDocument>();
        //
        //     for (int i = 0; i < 2; i++)
        //     {
        //         var upload = await testContext.Documents
        //             .UploadDocumentAsync(fileStream, $"ForDocumentGroupFile-{i}.pdf");
        //         var doc = await testContext.Documents.GetDocumentAsync(upload.Id).ConfigureAwait(false);
        //         documents.Add(doc);
        //     }
        //
        //     var documentGroup = await testContext.DocumentGroup.CreateDocumentGroupAsync(
        //         "CreateDocumentGroupTest", documents).ConfigureAwait(false);
        //
        //     Assert.IsTrue(documentGroup.Id.Length == 40);
        // }
        //
        // #endregion
    }
}
