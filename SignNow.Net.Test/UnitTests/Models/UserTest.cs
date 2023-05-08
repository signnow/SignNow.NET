using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net.Model;
using SignNow.Net.Test.FakeModels;

namespace UnitTests.Models
{
    [TestClass]
    public class UserTest : SignNowTestBase
    {
        [TestMethod]
        public void Deserialization()
        {
            var userFake = new UserSignNowFaker().Generate();
            var userFakeJson = TestUtils.SerializeToJsonFormatted(userFake);

            var user = TestUtils.DeserializeFromJson<User>(userFakeJson);

            TestUtils.Dump(user);
            Assert.That.JsonEqual(userFakeJson, user);
        }
    }
}
