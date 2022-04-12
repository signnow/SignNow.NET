using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SignNow.Net.Interfaces;
using SignNow.Net.Internal.Constants;
using SignNow.Net.Model;

namespace SignNow.Net.Service
{
    public class EventSubscriptionService : AuthorizedWebClientBase, IEventSubscriptionService
    {
        /// <summary>
        /// Creates new instance of <see cref="EventSubscriptionService"/>
        /// </summary>
        /// <param name="apiBaseUrl"><see cref="ApiUrl.ApiBaseUrl"/></param>
        /// <param name="token"><see cref="Token"/></param>
        public EventSubscriptionService(Uri apiBaseUrl, Token token) : this(apiBaseUrl, token, null)
        {
        }

        /// <inheritdoc cref="EventSubscriptionService(Uri, Token)"/>
        /// <param name="signNowClient"><see cref="ISignNowClient"/></param>
        protected internal EventSubscriptionService(Uri apiBaseUrl, Token token, ISignNowClient signNowClient) : base(apiBaseUrl, token, signNowClient)
        {
        }

        /// <inheritdoc />
        public async Task<EventSubscription> CreateEventSubscriptionAsync(EventSubscription createEvent, CancellationToken cancellationToken = default) => throw new NotImplementedException();

        /// <inheritdoc />
        public async Task<IEnumerable<EventSubscription>> GetEventSubscriptionAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public async Task<EventSubscription> UpdateEventSubscriptionAsync(CancellationToken cancellationToken = default) => throw new NotImplementedException();

        /// <inheritdoc />
        public async Task DeleteEventSubscriptionAsync(CancellationToken cancellationToken = default) => throw new NotImplementedException();
    }
}
