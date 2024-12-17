using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SignNow.Net.Examples
{
    public partial class UserExamples
    {
        [TestMethod]
        public async Task GetUserModifiedDocumentsAsync()
        {
            // get user modified documents
            var signNowDocuments = await testContext.Users
                .GetModifiedDocumentsAsync(perPage:25)
                .ConfigureAwait(false);

            // check if user is the owner of the modified documents
            var modifiedDocuments = signNowDocuments.ToList();
            foreach (var document in modifiedDocuments)
            {
                Assert.AreEqual(credentials.Login, document.Owner);
            }

            Assert.IsNotNull(modifiedDocuments.Count);
            Console.WriteLine($@"Total modified documents: {modifiedDocuments.Count}");
        }
    }
}
