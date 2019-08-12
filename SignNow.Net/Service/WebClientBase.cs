
using SignNow.Net._Internal.Service;
using SignNow.Net.Interface;
using System;

namespace SignNow.Net.Service
{
    public abstract class WebClientBase
    {
        protected ISignNowClient SignNowClient { get; private set; }
        public WebClientBase(Uri apiBaseUrl) : this(apiBaseUrl, null)
        {

        }

        protected WebClientBase(Uri apiBaseUrl, ISignNowClient signNowClient)
        {
            SignNowClient = signNowClient ?? new SignNowClient();
        }
    }
}
