using System;
using SignNow.Net.Internal.Extensions;
using SignNow.Net.Internal.Helpers;
using SignNow.Net.Model.Requests.EventSubscriptionBase;

namespace SignNow.Net.Model.Requests
{
    public sealed class CreateEventSubscription : AbstractEventSubscription
    {
        public CreateEventSubscription(EventType eventType, string entityId, Uri callbackUrl)
        {
            Guard.ArgumentNotNull(callbackUrl, nameof(callbackUrl));

            Event = eventType;
            EntityId = entityId.ValidateId();
            Attributes.CallbackUrl = callbackUrl;
        }
    }
}
