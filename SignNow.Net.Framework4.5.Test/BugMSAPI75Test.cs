using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net.Exceptions;
using SignNow.Net.Model;

namespace SignNow.Net.Framework4._5.Test
{
    [TestClass]
    public class BugMSAPI75Test
    {
        // Task MSAPI-75
        // Bug appear on .NET Framework 4.5 during GetTokenAsync request to https://api.signnow.com/ with any credentials
        // there is an exception => IOException: Authentication failed because the remote party has closed the transport stream in .NET 4.5 project
        // invalid client exception means that the bug did not occur during GetTokenAsync request
        [TestMethod]
        public async Task OAuth2Service_GetTokenAsync_SignNowExceptionInvalidClient()
        {
            var oAuth2Service = new OAuth2Service(
                    new Uri("https://api.signnow.com/"),
                    "fake client id",
                    "fake client secret"
                );

            var actualException = await Assert.ThrowsExceptionAsync<SignNowException>(
                    () => oAuth2Service.GetTokenAsync("fake username", "fake password", Scope.All));

            Assert.AreEqual("invalid_client", actualException.Message);
        }
    }
}