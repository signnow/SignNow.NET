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
            var user = new CredentialLoader(ApiBaseUrl).GetCredentials();
            var authObject = new OAuth2Service(user.Login, user.Password);
            var urlTask = authObject.GetAuthorizationUrlAsync(Model.Scope.All, "https://www.google.com");

            Assert.IsNotNull(urlTask.IsFaulted, $"Authorization Url retreiving error: {urlTask.Exception}");

            Task.WhenAll(urlTask);

            if (String.IsNullOrEmpty(urlTask.Result.ToString()))
                Assert.Fail("Authorization Url is empty");
        }
    }
}
