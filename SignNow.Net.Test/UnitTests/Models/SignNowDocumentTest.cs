using System;
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
            var injectedRoles = @"
            {
                'unique_id': '485a05488fb971644978d3ec943ff6c719bda83a',
                'signing_order': '1',
                'name': 'Signer 1'
            }";

            // use document template and inject roles
            var documentJson = JsonFixtures.DocumentTemplate.AsJsonObject();
            var roles = (JArray)documentJson["roles"];
            roles.Add(JsonFixtures.RoleTemplate.AsJsonObject());

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
            Assert.AreEqual(0, document.InviteRequests.Count);
        }
    }
}
