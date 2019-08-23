using SignNow.Net.Model;
using SignNow.Net.Test.Context;

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
            var oauth = new OAuth2Service(ApiBaseUrl, apiCreds.Login, apiCreds.Password);
            Token = oauth.GetTokenAsync(userCreds.Login, userCreds.Password, Scope.All).Result;
        }
    }
}