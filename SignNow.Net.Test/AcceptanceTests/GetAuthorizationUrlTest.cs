using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net.Test.Context;
using System;
using System.Threading.Tasks;

namespace SignNow.Net.Test.AcceptanceTests
{
    [TestClass]
    public class GetAuthorizationUrlTest : ApiTestBase
    {
        [TestMethod]
        public void AuthorizationUrlRetriever()
        {
            var redirectUrl = "https://www.google.com";

            redirectUrl = "https://localhost:44355/api/values";

            var user = new CredentialLoader(ApiBaseUrl).GetCredentials();
            var authObject = new OAuth2Service(user.Login, user.Password);
            var urlTask = authObject.GetAuthorizationUrlAsync(Model.Scope.All, redirectUrl);

            Assert.IsNotNull(urlTask.IsFaulted, $"Authorization Url retreiving error: {urlTask.Exception}");

            Task.WhenAll(urlTask);

            if (String.IsNullOrEmpty(urlTask.Result.ToString()))
                Assert.Fail("Authorization Url is empty");
        }
    }
}
