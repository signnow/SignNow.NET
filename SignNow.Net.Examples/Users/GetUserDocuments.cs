using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SignNow.Net.Examples
{
    public partial class UserExamples
    {
        [TestMethod]
        public async Task GetUserDocumentsAsync()
        {
            // Get user documents, first 25 documents
            var signNowDocuments = await testContext.Users
                .GetUserDocumentsAsync(perPage:25)
                .ConfigureAwait(false);

            // Check if the documents are owned by the user
            var userDocuments = signNowDocuments.ToList();
            foreach (var document in userDocuments)
            {
                Assert.AreEqual(credentials.Login, document.Owner);
            }

            Assert.IsNotNull(userDocuments.Count);
            Console.WriteLine($@"Total modified documents: {userDocuments.Count}");
        }
    }
}
