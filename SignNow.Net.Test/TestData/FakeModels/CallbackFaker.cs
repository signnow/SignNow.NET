using System;
using Bogus;
using SignNow.Net.Model;

namespace SignNow.Net.Test.FakeModels
{
    public class CallbackFaker : Faker<Callback>
    {
        /// <summary>
        /// Faker for <see cref="Callback"/>
        /// </summary>
        /// <example>
        /// This example shows Json representation.
        /// <code>
        /// {
        ///   "id": "callback_123456789",
        ///   "entity_id": "doc_987654321",
        ///   "callback_url": "https://example.com/webhook",
        ///   "initiator_id": "user_456789123",
        ///   "start_time": 1609459200,
        ///   "end_time": 1609459205,
        ///   "code": 200,
        ///   "event": "document.create",
        ///   "event_type": "document",
        ///   "application": "app_abcdef123456",
        ///   "response_body": "OK",
        ///   "error_message": null
        /// }
        /// </code>
        /// </example>
        public CallbackFaker()
        {
            StrictMode(true);
            
            RuleFor(o => o.Id, f => $"callback_{f.Random.Hash(10)}");
            RuleFor(o => o.EntityId, f => $"doc_{f.Random.Hash(10)}");
            RuleFor(o => o.CallbackUrl, f => new Uri(f.Internet.Url()));
            //RuleFor(o => o.InitiatorId, f => $"user_{f.Random.Hash(10)}");
            
            //var startTime = DateTimeOffset.UtcNow.AddDays(f => f.Random.Int(1, 30)).ToUnixTimeSeconds();
            //RuleFor(o => o.StartTime, f => startTime);
            //RuleFor(o => o.EndTime, f => f.Random.Bool(0.8f) ? startTime + f.Random.Int(1, 60) : (long?)null);
            
            //RuleFor(o => o.Code, f => f.PickRandom(200, 201, 204, 400, 401, 403, 404, 500, 502, 503));
            //RuleFor(o => o.Event, f => f.PickRandom("document.create", "document.update", "document.complete", "user.create", "document_group.create"));
            //RuleFor(o => o.EventType, f => f.PickRandom("document", "user", "document_group", "template"));
            //RuleFor(o => o.Application, f => f.Random.AlphaNumeric(40).ToLower());
            //RuleFor(o => o.ResponseBody, f => f.Random.Bool(0.7f) ? f.Lorem.Word() : null);
            //RuleFor(o => o.ErrorMessage, f => f.Random.Bool(0.2f) ? f.Lorem.Sentence() : null);
        }

        /// <summary>
        /// Creates a successful callback (HTTP 2xx).
        /// </summary>
        /// <returns>Faker configured for successful callbacks</returns>
        public static CallbackFaker Successful()
        {
            var faker = new CallbackFaker();
            //faker.RuleFor(o => o.Code, f => f.PickRandom(200, 201, 204));
            //faker.RuleFor(o => o.ErrorMessage, f => null);
            return faker;
        }

        /// <summary>
        /// Creates a failed callback (HTTP 4xx or 5xx).
        /// </summary>
        /// <returns>Faker configured for failed callbacks</returns>
        public static CallbackFaker Failed()
        {
            var faker = new CallbackFaker();
            //faker.RuleFor(o => o.Code, f => f.PickRandom(400, 401, 403, 404, 500, 502, 503));
            //faker.RuleFor(o => o.ErrorMessage, f => f.Lorem.Sentence());
            return faker;
        }

        /// <summary>
        /// Creates a callback for document events.
        /// </summary>
        /// <returns>Faker configured for document events</returns>
        public static CallbackFaker DocumentEvent()
        {
            var faker = new CallbackFaker();
            //faker.RuleFor(o => o.Event, f => f.PickRandom("document.create", "document.update", "document.complete"));
            //faker.RuleFor(o => o.EventType, f => "document");
            return faker;
        }

        /// <summary>
        /// Creates a callback for user events.
        /// </summary>
        /// <returns>Faker configured for user events</returns>
        public static CallbackFaker UserEvent()
        {
            var faker = new CallbackFaker();
            //faker.RuleFor(o => o.Event, f => f.PickRandom("user.create", "user.update"));
            //faker.RuleFor(o => o.EventType, f => "user");
            return faker;
        }
    }
}
