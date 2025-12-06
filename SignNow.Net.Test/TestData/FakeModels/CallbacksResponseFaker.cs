using System.Collections.Generic;
using Bogus;
using SignNow.Net.Model;
using SignNow.Net.Model.Responses;

namespace SignNow.Net.Test.FakeModels
{
    public class CallbacksResponseFaker : Faker<CallbacksResponse>
    {
        /// <summary>
        /// Faker for <see cref="CallbacksResponse"/>
        /// </summary>
        /// <example>
        /// This example shows Json representation.
        /// <code>
        /// {
        ///   "data": [
        ///     {
        ///       "id": "callback_123456789",
        ///       "entity_id": "doc_987654321",
        ///       "callback_url": "https://example.com/webhook",
        ///       "initiator_id": "user_456789123",
        ///       "start_time": 1609459200,
        ///       "end_time": 1609459205,
        ///       "code": 200,
        ///       "event": "document.create",
        ///       "event_type": "document",
        ///       "application": "app_abcdef123456",
        ///       "response_body": "OK",
        ///       "error_message": null
        ///     }
        ///   ],
        ///   "meta": {
        ///     "pagination": {
        ///       "total": 1,
        ///       "count": 1,
        ///       "per_page": 50,
        ///       "current_page": 1,
        ///       "total_pages": 1
        ///     }
        ///   }
        /// }
        /// </code>
        /// </example>
        public CallbacksResponseFaker()
        {
            StrictMode(true);
            
            RuleFor(o => o.Data, f => new CallbackFaker().Generate(f.Random.Int(1, 10)) as IReadOnlyList<Callback>);
            RuleFor(o => o.Meta, f => new MetaInfoFaker().Generate());
        }

        /// <summary>
        /// Creates a response with empty data.
        /// </summary>
        /// <returns>Faker configured for empty response</returns>
        public static CallbacksResponseFaker Empty()
        {
            var faker = new CallbacksResponseFaker();
            faker.RuleFor(o => o.Data, f => new List<Callback>() as IReadOnlyList<Callback>);
            return faker;
        }

        /// <summary>
        /// Creates a response with specified number of successful callbacks.
        /// </summary>
        /// <param name="count">Number of successful callbacks to generate</param>
        /// <returns>Faker configured for successful callbacks</returns>
        public static CallbacksResponseFaker WithSuccessfulCallbacks(int count)
        {
            var faker = new CallbacksResponseFaker();
            faker.RuleFor(o => o.Data, f => CallbackFaker.Successful().Generate(count) as IReadOnlyList<Callback>);
            return faker;
        }

        /// <summary>
        /// Creates a response with specified number of failed callbacks.
        /// </summary>
        /// <param name="count">Number of failed callbacks to generate</param>
        /// <returns>Faker configured for failed callbacks</returns>
        public static CallbacksResponseFaker WithFailedCallbacks(int count)
        {
            var faker = new CallbacksResponseFaker();
            faker.RuleFor(o => o.Data, f => CallbackFaker.Failed().Generate(count) as IReadOnlyList<Callback>);
            return faker;
        }
    }
}