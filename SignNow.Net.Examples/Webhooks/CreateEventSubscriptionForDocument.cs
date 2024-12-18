using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net.Model;
using SignNow.Net.Model.Requests;

namespace SignNow.Net.Examples
{
    [TestClass]
    public partial class CreateEventSubscriptionForDocument : ExamplesRunner
    {
        /// <summary>
        /// Allows to subscribe an external service(callback_url) to a specific event of user or document.
        /// As soon as a certain selected event from the List of event types occurs, SignNow sends a notification about it.
        /// </summary>
        /// <see cref="https://docs.signnow.com/docs/signnow/branches/v1.2/reference/operations/create-a-api-v-2-event"/>
        [TestMethod]
        public async Task CreateEventSubscriptionForDocumentAsync()
        {
            // Upload document with fields
            await using var fileStream = File.OpenRead(PdfWithSignatureField);
            var document = await testContext.Documents
                .UploadDocumentWithFieldExtractAsync(fileStream, "DocumentForEventSubscriptionCreate.pdf")
                .ConfigureAwait(false);

            // Using signNowContext lets create event subscription
            var myCallbackUrl = new Uri("https://signnow.com/callbackHandler");
            await testContext.Events
                .CreateEventSubscriptionAsync(new CreateEventSubscription(EventType.DocumentComplete, document.Id, myCallbackUrl))
                .ConfigureAwait(false);

            // Check for successful created event subscription
            // Gets information about all subscriptions to events made with a specific application
            // <see cref="https://docs.signnow.com/docs/signnow/reference/operations/list-api-v-2-events"/>
            var eventSubscriptionList = await testContext.Events
                .GetEventSubscriptionsAsync(new PagePaginationOptions { Page = 1, PerPage = 1})
                .ConfigureAwait(false);

            // Determining events total to get the latest event from whole events list
            var latestPage = eventSubscriptionList.Meta.Pagination.Total;

            // Getting event details from Events list
            var myLatestCreatedEvent = await testContext.Events
                .GetEventSubscriptionsAsync(new PagePaginationOptions { Page = latestPage, PerPage = 1})
                .ConfigureAwait(false);

            var myLatestEvent = myLatestCreatedEvent.Data.First();
            Assert.AreEqual(EventType.DocumentComplete, myLatestEvent.Event);
            Assert.AreEqual(myCallbackUrl, myLatestEvent.JsonAttributes.CallbackUrl);

            // Changing an existing event subscription
            // <see cref="https://docs.signnow.com/docs/signnow/reference/operations/update-a-api-v-2-event"/>
            var eventForUpdate = myLatestCreatedEvent.Data.First();
            eventForUpdate.JsonAttributes.CallbackUrl = new Uri("https://signnow.com/myNewCallbackHandler");

            var changedEvent = await testContext.Events
                .UpdateEventSubscriptionAsync(new UpdateEventSubscription(eventForUpdate))
                .ConfigureAwait(false);

            var updatedEvent = await testContext.Events
                .GetEventSubscriptionInfoAsync(changedEvent.Id)
                .ConfigureAwait(false);

            Assert.AreEqual(myLatestEvent.Id, updatedEvent.Id);
            Assert.AreEqual("https://signnow.com/myNewCallbackHandler", updatedEvent.JsonAttributes.CallbackUrl.AbsoluteUri);

            // Unsubscribes an external service (callback_url) from specific events of user or document
            // <see cref="https://docs.signnow.com/docs/signnow/reference/operations/delete-a-api-v-2-event"/>
            await testContext.Events
                .DeleteEventSubscriptionAsync(myLatestEvent.Id)
                .ConfigureAwait(false);

            // clean up
            DeleteTestDocument(document.Id);
        }
    }
}
