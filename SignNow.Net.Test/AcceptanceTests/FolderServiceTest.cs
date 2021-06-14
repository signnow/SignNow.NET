using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net.Model;
using UnitTests;

namespace AcceptanceTests
{
    [TestClass]
    public class FolderServiceTest : AuthorizedApiTestBase
    {
        [TestMethod]
        public void GetFolders()
        {
            var folders = SignNowTestContext.Folders.GetAllFoldersAsync().Result;

            Assert.IsInstanceOfType(folders, typeof(SignNowFolders));
            Assert.AreEqual("Root", folders.Name);
            Assert.IsTrue(folders.SystemFolder);

            Assert.IsTrue(folders.Folders.Any(f => f.Name == "Documents"));
            Assert.IsTrue(folders.Folders.Any(f => f.Name == "Archive"));
            Assert.IsTrue(folders.Folders.Any(f => f.Name == "Templates"));
        }
    }
}
