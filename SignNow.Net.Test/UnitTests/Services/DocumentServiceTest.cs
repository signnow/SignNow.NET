using System;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net.Exceptions;
using SignNow.Net.Model;
using SignNow.Net.Service;

namespace UnitTests
{
    [TestClass]
    public class DocumentServiceTest : SignNowTestBase
    {
        [TestMethod]
        public async Task MoveDocumentAsyncTest()
        {
            var service = new DocumentService(ApiBaseUrl, new Token(), SignNowClientMock("{\"result\":\"success\"}"));

            await service
                .MoveDocumentAsync(Faker.Random.Hash(40), Faker.Random.Hash(40))
                .ConfigureAwait(false);

            Assert.IsTrue(true);
        }

        [TestMethod]
        public async Task ThrowsExceptionForWrongParams()
        {
            var service = new DocumentService(new Token());

            var documentIdException = await Assert.ThrowsExceptionAsync<ArgumentException>(
                async () => await service
                    .MoveDocumentAsync("documentId", Faker.Random.Hash(40))
                    .ConfigureAwait(false)
                ).ConfigureAwait(false);

            var errorMessage1 = string.Format(CultureInfo.InvariantCulture, ExceptionMessages.InvalidFormatOfId, "documentId");
            StringAssert.Contains(documentIdException.Message, errorMessage1);
            Assert.AreEqual("documentId", documentIdException.ParamName);


            var folderIdException = await Assert.ThrowsExceptionAsync<ArgumentException>(
                async () => await service
                    .MoveDocumentAsync(Faker.Random.Hash(40), "folderId")
                    .ConfigureAwait(false)
            ).ConfigureAwait(false);

            var errorMessage2 = string.Format(CultureInfo.InvariantCulture, ExceptionMessages.InvalidFormatOfId, "folderId");
            StringAssert.Contains(folderIdException.Message, errorMessage2);
            Assert.AreEqual("folderId", folderIdException.ParamName);
        }
    }
}
