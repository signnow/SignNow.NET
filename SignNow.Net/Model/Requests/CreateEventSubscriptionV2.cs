using System;
using SignNow.Net.Extensions;
using SignNow.Net.Internal.Helpers;
using SignNow.Net.Model.Requests.EventSubscriptionBase;

namespace SignNow.Net.Model.Requests
{
    /// <summary>
    /// Request to create a v2 event subscription (webhook).
    /// </summary>
    public sealed class CreateEventSubscriptionV2 : AbstractEventSubscription
    {
        /// <summary>
        /// Constructs a new <see cref="CreateEventSubscriptionV2"/> request.
        /// </summary>
        /// <param name="eventType">Type of event to subscribe to.</param>
        /// <param name="entityId">ID of the entity the event applies to, e.g. a document, user, document group or template ID.</param>
        /// <param name="callbackUrl">URL SignNow will call when the event occurs.</param>
        /// <exception cref="System.ArgumentNullException">If <paramref name="callbackUrl"/> is null.</exception>
        public CreateEventSubscriptionV2(EventType eventType, string entityId, Uri callbackUrl)
        {
            Guard.ArgumentNotNull(callbackUrl, nameof(callbackUrl));
            Event = eventType;
            EntityId = entityId.ValidateId();
            Attributes.CallbackUrl = callbackUrl;
        }
    }
}
