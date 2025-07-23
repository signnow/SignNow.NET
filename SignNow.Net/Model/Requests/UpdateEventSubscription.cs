using System;
using Newtonsoft.Json;
using SignNow.Net.Internal.Extensions;
using SignNow.Net.Model.Requests.EventSubscriptionBase;

namespace SignNow.Net.Model.Requests
{
    public sealed class UpdateEventSubscription : AbstractEventSubscription
    {
        /// <summary>
        /// Identity of Event
        /// </summary>
        [JsonIgnore]
        public string Id { get; private set; }

        public UpdateEventSubscription(EventType eventType, string entityId, string eventId, Uri callbackUrl)
        {
            Id = eventId.ValidateId();
            EntityId = entityId.ValidateId();
            Event = eventType;
            Attributes.CallbackUrl = callbackUrl;
        }

        public UpdateEventSubscription(EventSubscription update)
        {
            Id = update.Id;
            EntityId = update.EntityUid;
            Attributes = new EventCreateAttributes
            {
                CallbackUrl = update.JsonAttributes.CallbackUrl,
                DocIdQueryParam = update.JsonAttributes.DocIdQueryParam,
                Headers = update.JsonAttributes.Headers,
                UseTls12 = update.JsonAttributes.UseTls12,
                IntegrationId = update.JsonAttributes.IntegrationId,
            };
            SecretKey = update.JsonAttributes.SecretKey;
        }
    }
}
