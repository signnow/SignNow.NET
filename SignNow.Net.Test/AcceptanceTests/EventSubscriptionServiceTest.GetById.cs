using System;
using System.Linq;
using System.Threading.Tasks;
using Bogus;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net.Model;
using SignNow.Net.Model.Requests;
using UnitTests;

namespace AcceptanceTests
{
    [TestClass]
    public class EventSubscriptionGetByIdTest : AuthorizedApiTestBase
    {
        [TestMethod]
        public async Task GetEventSubscriptionByIdAsync_ReturnsSubscriptionDetails()
        {
            var callbackUrl = new Uri($"https://example.com/{Faker.Random.Uuid()}");
            await SignNowTestContext.Events.CreateEventSubscriptionAsync(
                new CreateEventSubscription(EventType.DocumentUpdate, TestPdfDocumentId, callbackUrl)
            ).ConfigureAwait(false);

            var eventSubscriptions = await SignNowTestContext.Events
                .GetEventSubscriptionsListAsync(new GetEventSubscriptionsListOptions
                {
                    CallbackUrlFilter = CallbackUrlFilter.Like(callbackUrl.ToString()),
                })
                .ConfigureAwait(false);

            var createdSubscription = eventSubscriptions.Data.FirstOrDefault();
            Assert.IsNotNull(createdSubscription, "Should have at least one subscription");

            var retrievedSubscription = await SignNowTestContext.Events
                .GetEventSubscriptionByIdAsync(createdSubscription.Id)
                .ConfigureAwait(false);

            Assert.IsNotNull(retrievedSubscription);
            Assert.AreEqual(createdSubscription.Id, retrievedSubscription.Id);
            Assert.AreEqual(EventType.DocumentUpdate, retrievedSubscription.Event);
            Assert.AreEqual(TestPdfDocumentId, retrievedSubscription.EntityUid);
            Assert.AreEqual("post", retrievedSubscription.RequestMethod);
            Assert.AreEqual("callback", retrievedSubscription.Action);
            Assert.AreEqual(EventSubscriptionEntityType.Document, retrievedSubscription.EntityType);
            Assert.IsTrue(retrievedSubscription.Active);
            Assert.IsNotNull(retrievedSubscription.JsonAttributes);
            Assert.AreEqual(callbackUrl, retrievedSubscription.JsonAttributes.CallbackUrl);

            // todo: update to v2 DeleteEventSubscription
            await SignNowTestContext.Events
                .DeleteEventSubscriptionAsync(createdSubscription.Id)
                .ConfigureAwait(false);
        }
    }
}
