using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using SignNow.Net.Internal.Model;
using SignNow.Net.Test.FakeModels;

namespace UnitTests
{
    [TestClass]
    public class SignatureTest
    {
        [TestMethod]
        public void ShouldDeserializeFromJson()
        {
            var signatureFake = new SignatureFaker().Generate();
            var signatureFakeJson = JsonConvert.SerializeObject(signatureFake, Formatting.Indented);

            var signature = JsonConvert.DeserializeObject<Signature>(signatureFakeJson);
            var signatureJson = JsonConvert.SerializeObject(signature, Formatting.Indented);

            Assert.AreEqual(signatureFakeJson, signatureJson);
        }
    }
}
