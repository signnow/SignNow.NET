using Newtonsoft.Json;
using SignNow.Net.Model;
using SignNow.Net.Test.Context;
using System;

namespace SignNow.Net.Test
{
    public class AuthorizedApiTestBase : ApiTestBase
    {
        public Token Token { get; private set; }
        public AuthorizedApiTestBase()
        {
            var userCredentialsLoader = new CredentialLoader(ApplicationBaseUrl);
            var apiCredentialsLoader = new CredentialLoader(ApiBaseUrl);
            var apiCreds = apiCredentialsLoader.GetCredentials();
            var userCreds = userCredentialsLoader.GetCredentials();
            var oauth = new OAuth2Service(apiCreds.Login, apiCreds.Password);
            Token = oauth.GetTokenAsync(userCreds.Login, userCreds.Password, default).Result;//TODO: fix Scope parameter once the method is implemented
        }
    }
}
