using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net.Model;

namespace SignNow.Net.Examples
{
    public partial class FolderExamples
    {
        [TestMethod]
        public async Task GetAllFoldersAsync()
        {
            // get all folders
            var folders = await testContext.Folders
                .GetAllFoldersAsync()
                .ConfigureAwait(false);

            // check if the root folder contains the default folders
            Assert.IsInstanceOfType(folders, typeof(SignNowFolders));
            Assert.AreEqual("Root", folders.Name);
            Assert.IsTrue(folders.SystemFolder);

            Assert.IsTrue(folders.Folders.Any(f => f.Name == "Documents"));
            Assert.IsTrue(folders.Folders.Any(f => f.Name == "Archive"));
            Assert.IsTrue(folders.Folders.Any(f => f.Name == "Templates"));
        }
    }
}
