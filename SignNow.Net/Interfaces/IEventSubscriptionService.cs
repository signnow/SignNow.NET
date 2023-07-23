using System.Threading;
using System.Threading.Tasks;
using SignNow.Net.Model;
using SignNow.Net.Model.Requests;
using SignNow.Net.Model.Responses;

namespace SignNow.Net.Interfaces
{
    /// <summary>
    /// Interface for any operation with Events.
    /// Can be used for Create, Update, Delete and Get information about
    /// all subscriptions to events made with a specific application.
    /// </summary>
    public interface IEventSubscriptionService
    {
        /// <summary>
        /// Allows to subscribe an external service (callback_url) to a specific Event of User or Document.
        /// </summary>
        /// <param name="createEvent">Event details.</param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns></returns>
        Task CreateEventSubscriptionAsync(CreateEventSubscription createEvent, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets information about all subscriptions to Events made with a specific application.
        /// </summary>
        /// <param name="options">Query options</param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns></returns>
        Task<EventSubscriptionResponse> GetEventSubscriptionsAsync(IQueryToString options = default, CancellationToken cancellationToken = default);

        /// <summary>
        /// Allows users to get detailed info about one event subscription by its ID.
        /// </summary>
        /// <param name="eventId">Identity of event</param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns><see cref="EventSubscription"/> model</returns>
        Task<EventSubscription> GetEventSubscriptionInfoAsync(string eventId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Allows changing an existing Event subscription.
        /// </summary>
        /// <param name="updateEvent">Event details for update</param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns></returns>
        Task<EventUpdateResponse> UpdateEventSubscriptionAsync(UpdateEventSubscription updateEvent, CancellationToken cancellationToken = default);

        /// <summary>
        /// Unsubscribes an external service (callback_url) from specific Events of User or Document
        /// </summary>
        /// <param name="eventId">Specific event identity</param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns></returns>
        Task DeleteEventSubscriptionAsync(string eventId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Allows users to get the list of webhook events (events history) by the event subscription ID.
        /// </summary>
        /// <param name="eventId">Specific event identity</param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns>Events History page</returns>
        Task<EventHistoryListResponse> GetEventHistoryAsync(string eventId, CancellationToken cancellationToken = default);
    }
}
