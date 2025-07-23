using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.Requests
{
    [TestClass]
    public class UpdateUserOptionsTest : SignNowTestBase
    {
        [TestMethod]
        public void ShouldSerializeAllProperties()
        {
            var updateUserOptions = new SignNow.Net.Model.Requests.UpdateUserOptions
            {
                FirstName = "signNow",
                LastName = "SDK",
                OldPassword = "old-password",
                Password = "new-password",
                LogOutAll = false
            };

            var expected = @"{
              ""first_name"": ""signNow"",
              ""last_name"": ""SDK"",
              ""password"": ""new-password"",
              ""old_password"": ""old-password"",
              ""logout_all"": ""false""
            }";

            Assert.That.JsonEqual(updateUserOptions, expected);
        }
    }
}
