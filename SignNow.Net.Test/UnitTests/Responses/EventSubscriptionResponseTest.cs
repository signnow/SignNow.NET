using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net.Model;
using SignNow.Net.Model.Responses;

namespace UnitTests.Responses
{
    [TestClass]
    public class EventSubscriptionResponseTest
    {
        [TestMethod]
        public void ShouldProperDeserialize()
        {
            var jsonResponse = @"{
                ""data"": [
                    {
                        ""id"": ""8b784c586c6942c1bb04cf250400683779b1c49f"",
                        ""event"": ""document.complete"",
                        ""entity_id"": 40336962,
                        ""entity_unique_id"": ""5261f4a5c5fe47eaa68276366af40c259758fb30"",
                        ""action"": ""callback"",
                        ""json_attributes"": {
                            ""use_tls_12"": false,
                            ""docid_queryparam"": false,
                            ""callback_url"": ""https://my.callbackhandler.com/events/signnow?somearg=false""
                        },
                        ""application_name"": ""API Evaluation Application"",
                        ""created"": 1647337856
                    }
                ],
                ""meta"": {
                    ""pagination"": {
                        ""total"": 149,
                        ""count"": 15,
                        ""per_page"": 15,
                        ""current_page"": 2,
                        ""total_pages"": 10,
                        ""links"": {
                            ""previous"": ""https://api-eval.signnow.com/api/v2/events?page=1"",
                            ""next"": ""https://api-eval.signnow.com/api/v2/events?page=3""
                        }
                    }
                }
            }";

            var response = TestUtils.DeserializeFromJson<EventSubscriptionResponse>(jsonResponse);

            Assert.AreEqual(1, response.Data.Count);

            var subscription = response.Data[0];
            Assert.AreEqual("8b784c586c6942c1bb04cf250400683779b1c49f", subscription.Id);
            Assert.AreEqual(EventType.DocumentComplete, subscription.Event);
            Assert.AreEqual(40336962, subscription.EntityId);
            Assert.AreEqual("5261f4a5c5fe47eaa68276366af40c259758fb30", subscription.EntityUid);
            Assert.AreEqual("callback", subscription.Action);
            Assert.AreEqual(false, subscription.JsonAttributes.UseTls12);
            Assert.AreEqual(false, subscription.JsonAttributes.DocIdQueryParam);
            Assert.AreEqual(
                "https://my.callbackhandler.com/events/signnow?somearg=false",
                subscription.JsonAttributes.CallbackUrl.AbsoluteUri);
            Assert.AreEqual("API Evaluation Application", subscription.ApplicationName);
            Assert.AreEqual(149, response.Meta.Pagination.Total);
            Assert.AreEqual(15, response.Meta.Pagination.Count);
            Assert.AreEqual(15, response.Meta.Pagination.PerPage);
            Assert.AreEqual(2, response.Meta.Pagination.CurrentPage);
            Assert.AreEqual(10, response.Meta.Pagination.TotalPages);
            Assert.AreEqual("https://api-eval.signnow.com/api/v2/events?page=1", response.Meta.Pagination.Links.Previous.AbsoluteUri);
            Assert.AreEqual("https://api-eval.signnow.com/api/v2/events?page=3", response.Meta.Pagination.Links.Next.AbsoluteUri);
        }
    }
}
