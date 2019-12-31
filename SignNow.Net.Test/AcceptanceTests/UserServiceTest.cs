using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net.Service;
using SignNow.Net.Test;

namespace AcceptanceTests
{
    [TestClass]
    public class UserServiceTest : AuthorizedApiTestBase
    {
        [TestMethod]
        public void ShouldGetUserInfo()
        {
            var emailPattern = @"(?<userid>\S+)@(?<domain>\w+.\w+)";
            var userService = new UserService(Token);

            var userResponse = userService.GetCurrentUserAsync().Result;

            StringAssert.Matches(userResponse.Email, new Regex(emailPattern));
            Assert.IsTrue(userResponse.Active);
        }
    }
}
