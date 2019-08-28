using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net;
using SignNow.Net.Model;
using SignNow.Net.Test;
using SignNow.Net.Test.SignNow;
using System.Threading.Tasks;

namespace AcceptanceTests
{
    public partial class OAuth2ServiceTest : ApiTestBase
    {
        /// <summary>
        /// TO DO:  use GetAuthorizationUrlTest to retrieve authorization URL and get AUTH. CODE from HTTP-GET Responce
        /// on this URL and use this code in current test
        /// Make Positive Test
        /// </summary>

        [TestMethod]
        public void GetToken_Fail_WrongAuthCode()
        {
            var tokenTask = authObjectParam2.GetTokenAsync("wrong_auth_code", Scope.All);
            var tokenTaskExt = authObjectParam3.GetTokenAsync("wrong_auth_code", Scope.All);

            try
            {
                Task.WaitAll(tokenTask, tokenTaskExt);
                Assert.Fail($"Expected error \"{ErrorMessages.BadRequest}\" with wrong authorization code. Recieved Exception: {tokenTask.Exception}");
                Assert.Fail($"Expected error \"{ErrorMessages.BadRequest}\" with wrong authorization code. Recieved Exception: {tokenTaskExt.Exception}");
            }
            catch
            {
                Assert.AreEqual(tokenTask.Exception.InnerException.Message, ErrorMessages.BadRequest);
                Assert.AreEqual(tokenTaskExt.Exception.InnerException.Message, ErrorMessages.BadRequest);
            }
        }

        [TestMethod]
        public void GetToken_Fail_WrongClientSecret()
        {
            authObjectParam2 = new OAuth2Service(clientInfo.Login, "client_secret_wrong");
            authObjectParam3 = new OAuth2Service(ApiBaseUrl, clientInfo.Login, "client_secret_wrong");
            var tokenTask = authObjectParam2.GetTokenAsync(userCredentials.Login, userCredentials.Password, Scope.All);
            var tokenTaskExt = authObjectParam3.GetTokenAsync(userCredentials.Login, userCredentials.Password, Scope.All);

            try
            {
                Task.WaitAll(tokenTask, tokenTaskExt);
                Assert.Fail($"Expected error \"{ErrorMessages.BadRequest}\" with wrong user's client secret. Recieved Exception: {tokenTask.Exception}");
                Assert.Fail($"Expected error \"{ErrorMessages.BadRequest}\" with wrong user's client secret. Recieved Exception: {tokenTaskExt.Exception}");
            }
            catch
            {
                Assert.AreEqual(tokenTask.Exception.InnerException.Message, ErrorMessages.BadRequest);
                Assert.AreEqual(tokenTaskExt.Exception.InnerException.Message, ErrorMessages.BadRequest);
            }
        }
    }
}