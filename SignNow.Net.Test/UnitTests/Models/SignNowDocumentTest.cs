using System.Globalization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using SignNow.Net.Internal.Model;
using SignNow.Net.Model;

namespace UnitTests
{
    [TestClass]
    public class SignNowDocumentTest : SignNowTestBase
    {
        [TestMethod]
        public void ShouldDeserializeFromJson()
        {
            var json = @"{
                'id': 'a09b26feeba7ce70228afe6290f4445700b6f349',
                'user_id': '890d13607d89a7b3f6e67a14757d02ec00cf5eae',
                'document_name': 'pdf-test',
                'page_count': '1',
                'created': '1565787561',
                'updated': '1565858757',
                'original_filename': 'pdf-test.pdf',
                'origin_user_id': null,
                'origin_document_id': null,
                'owner': 'test.dotnet@signnow.com',
                'template': false,
                'roles': [
                    {
                        'unique_id': '485a05488fb971644978d3ec943ff6c719bda83a',
                        'signing_order': '1',
                        'name': 'Signer 1'
                    }
                ],
                'requests': []
            }";

            var document = JsonConvert.DeserializeObject<SignNowDocument>(json);

            Assert.AreEqual("a09b26feeba7ce70228afe6290f4445700b6f349", document.Id);
            Assert.AreEqual("890d13607d89a7b3f6e67a14757d02ec00cf5eae", document.UserId);
            Assert.IsNull(document.OriginDocumentId);
            Assert.IsNull(document.OriginUserId);
            Assert.AreEqual("pdf-test", document.Name);
            Assert.AreEqual("pdf-test.pdf", document.OriginalName);
            Assert.AreEqual(1, document.PageCount);
            Assert.AreEqual("2019-08-14 12:59:21Z", document.Created.ToString("u", CultureInfo.CurrentCulture));
            Assert.AreEqual("2019-08-15 08:45:57Z", document.Updated.ToString("u", CultureInfo.CurrentCulture));
            Assert.AreEqual("test.dotnet@signnow.com", document.Owner);
            Assert.IsFalse(document.IsTemplate);
            Assert.AreEqual(1, document.Roles.Count);
            CollectionAssert.AllItemsAreInstancesOfType(document.Roles, typeof(Role));
            Assert.AreEqual("Signer 1", document.Roles[0].Name);
            Assert.AreEqual(0, document.InviteRequests.Count);
        }
    }
}
