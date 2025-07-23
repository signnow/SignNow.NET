using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using SignNow.Net.Model;
using SignNow.Net.Test.FakeModels;

namespace UnitTests.Models
{
    [TestClass]
    public class RoleTest
    {
        [TestMethod]
        public void ShouldDeserializeFromJson()
        {
            var fakeRole = new RoleFaker().Generate();
            var jsonFake = JsonConvert.SerializeObject(fakeRole, Formatting.Indented);

            var role = JsonConvert.DeserializeObject<Role>(jsonFake);

            Assert.AreEqual(fakeRole.Id, role.Id);
            Assert.AreEqual(fakeRole.SigningOrder, role.SigningOrder);
            Assert.AreEqual(fakeRole.Name, role.Name);
        }
    }
}
