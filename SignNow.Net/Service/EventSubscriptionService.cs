using System;
using System.Threading;
using System.Threading.Tasks;
using SignNow.Net.Interfaces;
using SignNow.Net.Internal.Extensions;
using SignNow.Net.Model;
using SignNow.Net.Model.Requests;
using SignNow.Net.Model.Responses;

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
            Token.TokenType = TokenType.Bearer;
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
        public async Task<EventSubscriptionResponse> GetEventSubscriptionsAsync(IQueryToString options, CancellationToken cancellationToken = default)
        {
            Token.TokenType = TokenType.Basic;

            var query = options?.ToQueryString();
            var filters = string.IsNullOrEmpty(query)
                ? string.Empty
                : $"?{query}";

            var requestOptions = new GetHttpRequestOptions
            {
                RequestUrl = new Uri(ApiBaseUrl, $"/api/v2/events{filters}"),
                Token = Token
            };

            return await SignNowClient
                .RequestAsync<EventSubscriptionResponse>(requestOptions, cancellationToken)
                .ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<EventSubscription> GetEventSubscriptionInfoAsync(string eventId, CancellationToken cancellationToken = default)
        {
            Token.TokenType = TokenType.Bearer;
            var requestOptions = new GetHttpRequestOptions
            {
                RequestUrl = new Uri(ApiBaseUrl, $"/v2/dashboard/event-subscriptions/{eventId.ValidateId()}"),
                Token = Token
            };

            var responseData = await SignNowClient
                .RequestAsync<EventSubscriptionInfoResponse>(requestOptions, cancellationToken)
                .ConfigureAwait(false);

            return responseData.ResponseData;
        }

        /// <inheritdoc />
        public async Task<EventUpdateResponse> UpdateEventSubscriptionAsync(UpdateEventSubscription updateEvent, CancellationToken cancellationToken = default)
        {
            Token.TokenType = TokenType.Bearer;
            var requestOptions = new PutHttpRequestOptions
            {
                RequestUrl = new Uri(ApiBaseUrl, $"/api/v2/events/{updateEvent.Id.ValidateId()}"),
                Content = updateEvent,
                Token = Token
            };

            return await SignNowClient
                .RequestAsync<EventUpdateResponse>(requestOptions, cancellationToken)
                .ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task DeleteEventSubscriptionAsync(string eventId, CancellationToken cancellationToken = default)
        {
            Token.TokenType = TokenType.Basic;
            var requestOptions = new DeleteHttpRequestOptions
            {
                RequestUrl = new Uri(ApiBaseUrl, $"/api/v2/events/{eventId.ValidateId()}"),
                Token = Token
            };

            await SignNowClient
                .RequestAsync(requestOptions, cancellationToken)
                .ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<EventHistoryListResponse> GetEventHistoryAsync(string eventId, CancellationToken cancellationToken = default)
        {
            Token.TokenType = TokenType.Bearer;
            var requestOptions = new GetHttpRequestOptions
            {
                RequestUrl = new Uri(ApiBaseUrl, $"/v2/dashboard/event-subscriptions/{eventId.ValidateId()}/callbacks"),
                Token = Token
            };

            return await SignNowClient
                .RequestAsync<EventHistoryListResponse>(requestOptions, cancellationToken)
                .ConfigureAwait(false);
        }
    }
}
