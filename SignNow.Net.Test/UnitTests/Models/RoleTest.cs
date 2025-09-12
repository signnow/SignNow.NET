using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net.Model;
using SignNow.Net.Test.FakeModels;
using UnitTests;

namespace UnitTests.Models
{
    [TestClass]
    public class RoleTest
    {
        [TestMethod]
        public void ShouldDeserializeFromJson()
        {
            var fakeRole = new RoleFaker().Generate();
            var jsonFake = TestUtils.SerializeToJsonFormatted(fakeRole);

            var role = TestUtils.DeserializeFromJson<Role>(jsonFake);

            Assert.AreEqual(fakeRole.Id, role.Id);
            Assert.AreEqual(fakeRole.SigningOrder, role.SigningOrder);
            Assert.AreEqual(fakeRole.Name, role.Name);
        }
    }
}
