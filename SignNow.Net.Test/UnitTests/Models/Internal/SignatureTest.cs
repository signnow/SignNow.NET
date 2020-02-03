using System.Globalization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using SignNow.Net.Internal.Model;

namespace UnitTests
{
    [TestClass]
    public class SignatureTest
    {
        [TestMethod]
        public void ShouldDeserializeFromJson()
        {
            var json = @"{
                'id': '5cbcf9d0e5b0e77b78fef3202000220f01fea3cf',
                'user_id': 'fdbf8063a803daa7ec0d0f6476e78c4aa445a86f',
                'signature_request_id': '827a6dc8a83805f5969883304d2166b75ba19cf3',
                'email': 'signer@signnow.com',
                'created': '1580125749'
            }";

            var signature = JsonConvert.DeserializeObject<Signature>(json);

            Assert.AreEqual("5cbcf9d0e5b0e77b78fef3202000220f01fea3cf", signature.Id);
            Assert.AreEqual("fdbf8063a803daa7ec0d0f6476e78c4aa445a86f", signature.UserId);
            Assert.AreEqual("827a6dc8a83805f5969883304d2166b75ba19cf3", signature.SignatureRequestId);
            Assert.AreEqual("signer@signnow.com", signature.Email);
            Assert.AreEqual("2020-01-27 11:49:09Z", signature.Created.ToString("u", CultureInfo.CurrentCulture));
        }
    }
}
