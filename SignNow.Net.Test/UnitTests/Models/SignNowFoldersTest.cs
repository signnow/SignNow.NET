using System.Globalization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using SignNow.Net.Model;

namespace UnitTests
{
    [TestClass]
    public class SignNowFoldersTest : SignNowTestBase
    {
        [TestMethod]
        public void ShouldDeserializeFromJson()
        {
            var folderJson = @"{
            ""id"": ""e1d8d63ba51c4009ab8241f249c908b0fd5a5e48"",
            ""parent_id"": ""a7138ccc971e98080bfa999cc32d4bef4cca51a9"",
            ""user_id"": ""40204b3344983368bb16d61f8550f8b5edfd719b"",
            ""name"": ""Documents"",
            ""created"": ""1566560035"",
            ""shared"": false,
            ""document_count"": ""150"",
            ""template_count"": ""10"",
            ""folder_count"": ""20""
            }";

            var folder = JsonConvert.DeserializeObject<Folder>(folderJson);

            Assert.AreEqual("e1d8d63ba51c4009ab8241f249c908b0fd5a5e48", folder.Id);
            Assert.AreEqual("a7138ccc971e98080bfa999cc32d4bef4cca51a9", folder.ParentId);
            Assert.AreEqual("40204b3344983368bb16d61f8550f8b5edfd719b", folder.UserId);
            Assert.AreEqual("Documents", folder.Name);
            Assert.AreEqual("2019-08-23 11:33:55Z", folder.Created.ToString("u", CultureInfo.CurrentCulture));
            Assert.AreEqual(false, folder.Shared);
            Assert.AreEqual(150, folder.TotalDocuments);
            Assert.AreEqual(20, folder.TotalFolders);
            Assert.AreEqual(10, folder.TotalTemplates);
        }
    }
}
