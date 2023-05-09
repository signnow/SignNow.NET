using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net.Model;
using SignNow.Net.Model.Requests;

namespace UnitTests.Requests
{
    [TestClass]
    public class CreateEventSubscriptionTest
    {
        [TestMethod]
        public void ShouldProperCreateJsonRequest()
        {
            var option = new CreateEventSubscription
            {
                Event = EventType.DocumentComplete,
                EntityId = "827a6dc8a83805f5961234304d2166b75ba19cf3",
                Attributes = new EventAttributes
                {
                    UseTls12 = true,
                    CallbackUrl = new Uri("http://localhost/callback")
                },
                SecretKey = "12345"

            };

            var actual = $@"{{
                ""action"": ""callback"",
                ""event"": ""document.complete"",
                ""entity_id"": ""827a6dc8a83805f5961234304d2166b75ba19cf3"",
                ""attributes"": {{
                    ""use_tls_12"": true,
                    ""docid_queryparam"": false,
                    ""callback_url"": ""http://localhost/callback""
                }},
                ""secret_key"": ""12345""
            }}";

            Assert.That.JsonEqual(actual, option);
        }
    }
}
