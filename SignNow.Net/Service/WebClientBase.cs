using SignNow.Net.Internal.Service;
using SignNow.Net.Interfaces;
using System;
using SignNow.Net.Model;

namespace SignNow.Net.Service
{
    public abstract class WebClientBase
    {
        /// <summary>
        /// signNow HTTP Client.
        /// </summary>
        protected static ISignNowClient SignNowClient { get; private set; }

        /// <summary>
        /// User Access token.
        /// </summary>
        public Token Token { get; set; }

        /// <summary>
        /// Base signNow api URL.
        /// </summary>
        protected Uri ApiBaseUrl { get; set; }

        /// <summary>
        /// Base Web Client for HTTP calls
        /// </summary>
        /// <param name="apiBaseUrl">Base signNow api URL</param>
        /// <param name="token">User Access token</param>
        /// <param name="signNowClient">signNow HTTP Client</param>
        protected WebClientBase(Uri apiBaseUrl, Token token, ISignNowClient signNowClient)
        {
            ApiBaseUrl = apiBaseUrl;
            Token = token;
            SignNowClient = signNowClient ?? new SignNowClient();
        }
    }
}
