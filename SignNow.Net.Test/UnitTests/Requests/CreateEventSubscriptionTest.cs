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
            var option = new CreateEventSubscription(
                EventType.DocumentComplete,
                "827a6dc8a83805f5961234304d2166b75ba19cf3",
                new Uri("http://localhost/callback"))
            {
                SecretKey = "12345",
                Attributes =
                {
                    UseTls12 = true
                }
            };

            var actual = $@"{{
                ""action"": ""callback"",
                ""event"": ""document.complete"",
                ""entity_id"": ""827a6dc8a83805f5961234304d2166b75ba19cf3"",
                ""attributes"": {{
                    ""delete_access_token"": true,
                    ""callback"": ""http://localhost/callback"",
                    ""use_tls_12"": true,
                    ""docid_queryparam"": false
                }},
                ""secret_key"": ""12345""
            }}";

            Assert.That.JsonEqual(actual, option);
        }
    }
}
