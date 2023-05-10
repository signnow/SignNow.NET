using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SignNow.Net.Interfaces;
using SignNow.Net.Model;
using SignNow.Net.Model.Requests;

namespace SignNow.Net.Service
{
    public class EventSubscriptionService : WebClientBase, IEventSubscriptionService
    {
        /// <summary>
        /// Creates new instance of <see cref="EventSubscriptionService"/>
        /// </summary>
        /// <param name="baseApiUrl">Base signNow API URL</param>
        /// <param name="token">Access token</param>
        /// <param name="signNowClient">signNow Http client</param>
        public EventSubscriptionService(Uri baseApiUrl, Token token, ISignNowClient signNowClient = null)
            : base(baseApiUrl, token, signNowClient)
        {
        }

        /// <inheritdoc />
        public async Task CreateEventSubscriptionAsync(CreateEventSubscription createEvent, CancellationToken cancellationToken = default)
        {
            var requestOptions = new PostHttpRequestOptions
            {
                RequestUrl = new Uri(ApiBaseUrl, "/api/v2/events"),
                Content = createEvent,
                Token = Token
            };

            await SignNowClient
                .RequestAsync(requestOptions, cancellationToken)
                .ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<EventSubscription>> GetEventSubscriptionAsync(CancellationToken cancellationToken = default)
        {
            var basicToken = Token;
            basicToken.TokenType = TokenType.Basic;

            var requestOptions = new GetHttpRequestOptions
            {
                RequestUrl = new Uri(ApiBaseUrl, "/api/v2/events"),
                Token = basicToken
            };

            return await SignNowClient
                .RequestAsync<List<EventSubscription>>(requestOptions, cancellationToken)
                .ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<EventSubscription> UpdateEventSubscriptionAsync(CancellationToken cancellationToken = default) => throw new NotImplementedException();

        /// <inheritdoc />
        public async Task DeleteEventSubscriptionAsync(CancellationToken cancellationToken = default) => throw new NotImplementedException();
    }
}
