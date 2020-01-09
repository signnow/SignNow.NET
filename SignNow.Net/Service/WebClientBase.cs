
using SignNow.Net.Internal.Service;
using SignNow.Net.Interfaces;
using System;

namespace SignNow.Net.Service
{
    public abstract class WebClientBase
    {
        /// <summary>
        /// SignNow HTTP Client.
        /// </summary>
        /// <value><see cref="SignNowClient"/></value>
        protected ISignNowClient SignNowClient { get; private set; }

        /// <summary>
        /// Base SignNow api URL.
        /// </summary>
        /// <value><see cref="Uri"/></value>
        protected Uri ApiBaseUrl { get; set; }

        /// <summary>
        /// Base Web Client for HTTP calls
        /// </summary>
        /// <param name="apiBaseUrl">Base SignNow api URL</param>
        /// <param name="signNowClient">SignNow HTTP Client</param>
        protected WebClientBase(Uri apiBaseUrl, ISignNowClient signNowClient)
        {
            ApiBaseUrl = apiBaseUrl;
            SignNowClient = signNowClient ?? new SignNowClient();
        }
    }
}
