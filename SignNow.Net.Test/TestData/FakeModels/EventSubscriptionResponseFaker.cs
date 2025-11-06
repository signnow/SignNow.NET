using System;
using System.Collections.Generic;
using Bogus;
using SignNow.Net.Model;
using SignNow.Net.Model.Responses;

namespace SignNow.Net.Test.FakeModels
{
    /// <summary>
    /// Faker <see cref="EventSubscriptionResponse"/>
    /// </summary>
    public class EventSubscriptionResponseFaker : Faker<EventSubscriptionResponse>
    {
        /// <summary>
        /// Creates new instance of <see cref="EventSubscriptionResponse"/> fake object.
        /// </summary>
        /// <example>
        /// This example shows Json representation.
        /// <code>
        /// {
        ///   "data": [
        ///     {
        ///       "id": "8b784c586c6942c1bb04cf250400683779b1c49f",
        ///       "event": "document.complete",
        ///       "entity_id": 40336962,
        ///       "entity_unique_id": "5261f4a5c5fe47eaa68276366af40c259758fb30",
        ///       "action": "callback",
        ///       "json_attributes": {
        ///         "use_tls_12": false,
        ///         "docid_queryparam": false,
        ///         "callback_url": "https://my.callbackhandler.com/events/signnow"
        ///       },
        ///       "application_name": "API Evaluation Application",
        ///       "created": 1647337856
        ///     }
        ///   ],
        ///   "meta": {
        ///     "pagination": {
        ///       "total": 149,
        ///       "count": 15,
        ///       "per_page": 15,
        ///       "current_page": 2,
        ///       "total_pages": 10
        ///     }
        ///   }
        /// }
        /// </code>
        /// </example>
        public EventSubscriptionResponseFaker()
        {
            Rules((f, o) =>
            {
                o.Data = new EventSubscriptionFaker().Generate(f.Random.Int(1, 5));
                o.Meta = new MetaInfoFaker().Generate();
            });
        }
    }

    /// <summary>
    /// Faker <see cref="EventSubscription"/>
    /// </summary>
    public class EventSubscriptionFaker : Faker<EventSubscription>
    {
        /// <summary>
        /// Creates new instance of <see cref="EventSubscription"/> fake object.
        /// </summary>
        public EventSubscriptionFaker()
        {
            Rules((f, o) =>
            {
                o.Id = f.Random.Hash(40);
                o.Event = f.PickRandom<EventType>();
                o.EntityId = f.Random.Int(10000000, 99999999);
                o.EntityUid = f.Random.Hash(40);
                o.Action = "callback";
                o.JsonAttributes = new EventAttributesFaker().Generate();
                o.ApplicationName = f.Company.CompanyName();
                o.Created = f.Date.Recent().ToUniversalTime();
            });
        }
    }

    /// <summary>
    /// Faker <see cref="EventAttributes"/>
    /// </summary>
    public class EventAttributesFaker : Faker<EventAttributes>
    {
        /// <summary>
        /// Creates new instance of <see cref="EventAttributes"/> fake object.
        /// </summary>
        public EventAttributesFaker()
        {
            Rules((f, o) =>
            {
                o.UseTls12 = f.Random.Bool();
                o.DocIdQueryParam = f.Random.Bool();
                o.CallbackUrl = new Uri(f.Internet.Url());
                o.IntegrationId = f.Random.Hash(40);
                o.Headers = new EventAttributeHeadersFaker().Generate();
            });
        }
    }

    /// <summary>
    /// Faker <see cref="EventAttributeHeaders"/>
    /// </summary>
    public class EventAttributeHeadersFaker : Faker<EventAttributeHeaders>
    {
        /// <summary>
        /// Creates new instance of <see cref="EventAttributeHeaders"/> fake object.
        /// </summary>
        public EventAttributeHeadersFaker()
        {
            Rules((f, o) =>
            {
                o.StringHead = f.Lorem.Word();
                o.IntHead = f.Random.Int(1, 100);
                o.BoolHead = f.Random.Bool();
                o.FloatHead = f.Random.Float(1, 100);
            });
        }
    }
}