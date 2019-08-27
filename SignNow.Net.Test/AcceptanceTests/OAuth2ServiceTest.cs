using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net;
using SignNow.Net.Test;
using SignNow.Net.Test.Context;

namespace AcceptanceTests
{
    [TestClass]
    public partial class OAuth2ServiceTest : ApiTestBase
    {
        private string authCode = "fac566fb5d926c7dd590f6eea7f8b6c10cbe469b";

        private CredentialModel clientInfo, userCredentials;
        private OAuth2Service authObjectParam2, authObjectParam3;

        [TestInitialize]
        public void TestInitialize()
        {
            clientInfo = new CredentialLoader(ApiBaseUrl).GetCredentials();
            userCredentials = new CredentialLoader(ApplicationBaseUrl).GetCredentials();
            authObjectParam2 = new OAuth2Service(clientInfo.Login, clientInfo.Password);
            authObjectParam3 = new OAuth2Service(ApiBaseUrl, clientInfo.Login, clientInfo.Password);
        }
    }
}
