using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using SignNow.Net.Model.FieldContents;
using SignNow.Net.Test.FakeModels;
using UnitTests;

namespace UnitTests.Models
{
    [TestClass]
    public class SignatureTest
    {
        [TestMethod]
        public void ShouldDeserializeFromJson()
        {
            var signatureFake = new SignatureContentFaker().Generate();
            var signatureFakeJson = JsonConvert.SerializeObject(signatureFake, Formatting.Indented);

            var signature = JsonConvert.DeserializeObject<SignatureContent>(signatureFakeJson);

            Assert.That.JsonEqual(signatureFakeJson, signature);
        }
    }
}
