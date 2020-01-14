using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using SignNow.Net.Internal.Model;

namespace UnitTests
{
    [TestClass]
    public class RoleTest
    {
        [TestMethod]
        public void ShouldDeserializeFromJson()
        {
            var json = "{" +
                       "\"unique_id\": \"485a05488fb971644978d3ec943ff6c719bda83a\"," +
                       "\"signing_order\": \"1\"," +
                       "\"name\": \"Signer 1\"" +
                       "}";

            var role = JsonConvert.DeserializeObject<Role>(json);

            Assert.AreEqual("485a05488fb971644978d3ec943ff6c719bda83a", role.Id);
            Assert.AreEqual(1, role.SigningOrder);
            Assert.AreEqual("Signer 1", role.Name);
        }
    }
}
