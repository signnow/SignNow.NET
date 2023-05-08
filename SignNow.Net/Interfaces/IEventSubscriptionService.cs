using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SignNow.Net.Model;

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
        Task CreateEventSubscriptionAsync(EventSubscription createEvent, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets information about all subscriptions to Events made with a specific application.
        /// </summary>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns></returns>
        Task<IEnumerable<EventSubscription>> GetEventSubscriptionAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Allows changing an existing Event subscription.
        /// </summary>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns></returns>
        Task<EventSubscription> UpdateEventSubscriptionAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Unsubscribes an external service (callback_url) from specific Events of User or Document
        /// </summary>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns></returns>
        Task DeleteEventSubscriptionAsync(CancellationToken cancellationToken = default);
    }
}
