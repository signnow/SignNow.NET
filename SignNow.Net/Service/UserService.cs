using System;
using System.Threading;
using System.Threading.Tasks;
using SignNow.Net.Interfaces;
using SignNow.Net.Internal.Constants;
using SignNow.Net.Model;

namespace SignNow.Net.Service
{
    public class UserService : AuthorizedWebClientBase, IUserService
    {
        public UserService(Token token) : this(ApiUrl.ApiBaseUrl, token)
        {
        }

        private UserService(Uri baseApiUrl, Token token) : base(baseApiUrl, token)
        {
        }

        protected internal UserService(Uri baseApiUrl, Token token, ISignNowClient client) : base(baseApiUrl, token,
            client)
        {
        }

        /// <inheritdoc />
        public async Task<User> GetCurrentUserAsync(CancellationToken cancellationToken = default)
        {
            var requestOptions = new GetHttpRequestOptions
            {
                RequestUrl = new Uri(ApiBaseUrl, "/user"),
                Token = Token
            };

            return await SignNowClient.RequestAsync<User>(requestOptions, cancellationToken).ConfigureAwait(false);
        }
    }
}
