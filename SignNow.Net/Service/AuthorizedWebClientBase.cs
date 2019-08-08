using SignNow.Net.Interface;
using SignNow.Net.Model;
using System;

namespace SignNow.Net.Service
{
    public abstract class AuthorizedWebClientBase : WebClientBase
    {
        public AuthorizedWebClientBase(Uri apiBaseUrl, Token token) : this(apiBaseUrl, token, null)
        {

        }
        protected AuthorizedWebClientBase(Uri apiBaseUrl, Token token, ISignNowClient signNowClient) : base(apiBaseUrl, signNowClient)
        {

        }
    }
}
