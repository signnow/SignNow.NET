using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net.Model;
using SignNow.Net.Model.Requests;

namespace UnitTests.Requests
{
    [TestClass]
    public class UpdateEventSubscriptionTest
    {
        [TestMethod]
        public void ShouldProperCreateJsonRequest()
        {
            var option = new UpdateEventSubscription(
                EventType.DocumentUpdate,
                "5261f4a5c5fe47eaa68276366af40c259758fb30",
                "827a6dc8a83805f5961234304d2166b75ba19cf3",
                new Uri("http://localhost/callback"))
            {
                SecretKey = "12345",
                Attributes =
                {
                    UseTls12 = true
                }
            };

            var expected = $@"{{
                ""action"": ""callback"",
                ""event"": ""document.update"",
                ""entity_id"": ""5261f4a5c5fe47eaa68276366af40c259758fb30"",
                ""attributes"": {{
                    ""delete_access_token"": true,
                    ""callback"": ""http://localhost/callback"",
                    ""use_tls_12"": true,
                    ""docid_queryparam"": false
                }},

                ""secret_key"": ""12345""
            }}";

            Assert.That.JsonEqual(expected, option);
        }

        [TestMethod]
        public void ShouldProperCreateJsonRequestForEmptyProperties()
        {
            var option = new UpdateEventSubscription(
                EventType.DocumentUpdate,
                "5261f4a5c5fe47eaa68276366af40c259758fb30",
                "827a6dc8a83805f5961234304d2166b75ba19cf3",
                new Uri("http://localhost/callback"));

            var expected = $@"{{
                ""action"": ""callback"",
                ""event"": ""document.update"",
                ""entity_id"": ""5261f4a5c5fe47eaa68276366af40c259758fb30"",
                ""attributes"": {{
                    ""delete_access_token"": true,
                    ""callback"": ""http://localhost/callback"",
                    ""use_tls_12"": false,
                    ""docid_queryparam"": false
                }}
            }}";

            Assert.That.JsonEqual(expected, option);
        }
    }
}
