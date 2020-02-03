using System.Globalization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SignNow.Net.Internal.Model;
using SignNow.Net.Model;
using SignNow.Net.Test;
using SignNow.Net.Test.Constants;
using SignNow.Net.Test.Extensions;

namespace UnitTests
{
    [TestClass]
    public class SignNowDocumentTest : SignNowTestBase
    {
        [TestMethod]
        public void ShouldDeserializeFromJson()
        {
            var documentJson = JsonFixtures.DocumentTemplate.AsJsonObject();

            // inject one role
            var roles = (JArray)documentJson["roles"];
            roles.Add(JsonFixtures.RoleTemplate.AsJsonObject());

            // inject one signature
            var signature = (JArray)documentJson["signatures"];
            signature.Add(JsonFixtures.SignatureTemplate.AsJsonObject());

            // inject one freeform invite
            var freeformInvite = (JArray)documentJson["requests"];
            freeformInvite.Add(JsonFixtures.FreeFormInviteTemplate.AsJsonObject());

            var document = JsonConvert.DeserializeObject<SignNowDocument>(documentJson.ToString());

            // Document assertions
            Assert.AreEqual("documentId000000000000000000000000000000", document.Id);
            Assert.AreEqual("userId0000000000000000000000000000000000", document.UserId);
            Assert.IsNull(document.OriginDocumentId);
            Assert.IsNull(document.OriginUserId);
            Assert.AreEqual("pdf-test", document.Name);
            Assert.AreEqual("pdf-test.pdf", document.OriginalName);
            Assert.AreEqual(1, document.PageCount);
            Assert.AreEqual("2019-08-14 12:59:21Z", document.Created.ToString("u", CultureInfo.CurrentCulture));
            Assert.AreEqual("2019-08-15 08:45:57Z", document.Updated.ToString("u", CultureInfo.CurrentCulture));
            Assert.AreEqual("test.user@signnow.com", document.Owner);
            Assert.IsFalse(document.IsTemplate);

            // Roles assertions
            Assert.AreEqual(1, document.Roles.Count);
            CollectionAssert.AllItemsAreInstancesOfType(document.Roles, typeof(Role));
            Assert.AreEqual("roleUniqueId0000000000000000000000000000", document.Roles[0].Id);
            Assert.AreEqual(1, document.Roles[0].SigningOrder);
            Assert.AreEqual("Signer 1", document.Roles[0].Name);

            // Signature assertions
            Assert.AreEqual(1, document.Signatures.Count);
            CollectionAssert.AllItemsAreInstancesOfType(document.Signatures, typeof(Signature));
            Assert.AreEqual("signatureId00000000000000000000000000000", document.Signatures[0].Id);
            Assert.AreEqual("userId0000000000000000000000000000000000", document.Signatures[0].UserId);
            Assert.AreEqual("signatureRequestId0000000000000000000000", document.Signatures[0].SignatureRequestId);
            Assert.AreEqual("signer@signnow.com", document.Signatures[0].Email);
            Assert.AreEqual("2020-01-27 11:49:09Z", document.Signatures[0].Created.ToString("u", CultureInfo.CurrentCulture));

            // Freeform invite assertions
            Assert.AreEqual(1, document.InviteRequests.Count);
            CollectionAssert.AllItemsAreInstancesOfType(document.InviteRequests, typeof(FreeformInvite));
            Assert.AreEqual("freeformInviteId000000000000000000000000", document.InviteRequests[0].Id);
            Assert.AreEqual("userId0000000000000000000000000000000000", document.InviteRequests[0].UserId);
            Assert.AreEqual("signatureId00000000000000000000000000000", document.InviteRequests[0].SignatureId);
            Assert.AreEqual("2020-01-15 12:09:38Z", document.InviteRequests[0].Created.ToString("u", CultureInfo.CurrentCulture));
            Assert.AreEqual("test.user@signnow.com", document.InviteRequests[0].Owner);
            Assert.AreEqual("signer@signnow.com", document.InviteRequests[0].Signer);
            Assert.IsNull(document.InviteRequests[0].IsCanceled);
        }
    }
}
