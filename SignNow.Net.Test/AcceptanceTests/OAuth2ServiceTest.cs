using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net;
using SignNow.Net.Test;
using SignNow.Net.Test.Context;

namespace AcceptanceTests
{
    [TestClass]
    public partial class OAuth2ServiceTest : ApiTestBase
    {
        private readonly CredentialModel clientInfo, userCredentials;
        private OAuth2Service authObjectParam2, authObjectParam3;

        public OAuth2ServiceTest()
        {
            clientInfo = new CredentialLoader(ApiBaseUrl).GetCredentials();
            userCredentials = new CredentialLoader(ApplicationBaseUrl).GetCredentials();
            authObjectParam2 = new OAuth2Service(clientInfo.Login, clientInfo.Password);
            authObjectParam3 = new OAuth2Service(ApiBaseUrl, clientInfo.Login, clientInfo.Password);
        }
    }
}