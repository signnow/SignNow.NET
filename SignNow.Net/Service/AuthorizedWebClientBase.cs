using SignNow.Net.Interfaces;
using SignNow.Net.Model;
using System;

namespace SignNow.Net.Service
{
    public class AuthorizedWebClientBase : WebClientBase
    {
        protected Token Token { get; set; }
        public AuthorizedWebClientBase(Uri apiBaseUrl, Token token) : this(apiBaseUrl, token, null)
        {

        }
        protected AuthorizedWebClientBase(Uri apiBaseUrl, Token token, ISignNowClient signNowClient) : base(apiBaseUrl, signNowClient)
        {
            Token = token;
        }
    }
}
