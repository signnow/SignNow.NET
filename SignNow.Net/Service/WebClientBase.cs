
using SignNow.Net.Internal.Service;
using SignNow.Net.Interfaces;
using System;

namespace SignNow.Net.Service
{
    public class WebClientBase
    {
        protected ISignNowClient SignNowClient { get; private set; }
        protected Uri ApiBaseUrl { get; set; }
        public WebClientBase(Uri apiBaseUrl) : this(apiBaseUrl, null)
        {

        }

        protected WebClientBase(Uri apiBaseUrl, ISignNowClient signNowClient)
        {
            ApiBaseUrl = apiBaseUrl;
            SignNowClient = signNowClient ?? new SignNowClient();
        }
    }
}
