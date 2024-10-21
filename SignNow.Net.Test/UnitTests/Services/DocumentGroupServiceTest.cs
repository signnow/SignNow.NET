using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net.Model;
using SignNow.Net.Model.Responses;
using SignNow.Net.Service;
using SignNow.Net.Test.FakeModels;

namespace UnitTests.Services
{
    [TestClass]
    public class DocumentGroupServiceTest : SignNowTestBase
    {
        [TestMethod]
        public async Task CreateDocumentGroupAsyncTest()
        {
            var service = new DocumentGroupService(ApiBaseUrl, new Token(),
                SignNowClientMock("{\"id\":\"add9e5af17ad0876ed1ec327cc86209d0377181d\"}"));

            var documents = new SignNowDocumentFaker().Generate(5);
            var response = await service.CreateDocumentGroupAsync("test group", documents).ConfigureAwait(false);

            Assert.IsInstanceOfType(response, typeof(DocumentGroupCreateResponse));
            Assert.AreEqual("add9e5af17ad0876ed1ec327cc86209d0377181d", response.Id);
        }
    }
}
