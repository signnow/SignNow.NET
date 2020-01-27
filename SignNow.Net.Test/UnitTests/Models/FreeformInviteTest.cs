using System.Globalization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using SignNow.Net.Model;

namespace UnitTests
{
    [TestClass]
    public class FreeformInviteTest
    {
        [TestMethod]
        public void ShouldDeserializeFromJson()
        {
            var json = @"
                {
                    'unique_id': '827a6dc8a83805f5961234304d2166b75ba19cf3',
                    'id': '827a6dc8a83805f5961234304d2166b75ba19cf3',
                    'user_id': '40204b3344984768bb16d61f8550f8b5edfd719a',
                    'created': '1579090178',
                    'originator_email': 'owner@signnow.com',
                    'signer_email': 'signer@signnow.com',
                    'canceled': null,
                    'signature_id': '5abc19d0e5b0e77b78fef3202000220f01fea3cf'
                }";

            var response = JsonConvert.DeserializeObject<FreeformInvite>(json);

            Assert.AreEqual("827a6dc8a83805f5961234304d2166b75ba19cf3", response.Id);
            Assert.AreEqual("40204b3344984768bb16d61f8550f8b5edfd719a", response.UserId);
            Assert.AreEqual("5abc19d0e5b0e77b78fef3202000220f01fea3cf", response.SignatureId);
            Assert.AreEqual("2020-01-15 12:09:38Z", response.Created.ToString("u", CultureInfo.CurrentCulture));
            Assert.AreEqual("owner@signnow.com", response.Owner);
            Assert.AreEqual("signer@signnow.com", response.Signer);
            Assert.IsNull(response.IsCanceled);
        }
    }
}
