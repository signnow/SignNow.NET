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
            var signatureFakeJson = TestUtils.SerializeToJsonFormatted(signatureFake);

            var signature = TestUtils.DeserializeFromJson<SignatureContent>(signatureFakeJson);

            Assert.That.JsonEqual(signatureFakeJson, signature);
        }
    }
}
