using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net.Model;
using SignNow.Net.Model.Requests;
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

        [TestMethod]
        public async Task ThrowsExceptionForWrongParamsTest()
        {
            var service = new DocumentGroupService(ApiBaseUrl, new Token(), SignNowClientMock("{}"));

            var options = new LimitOffsetOptions
            {
                Limit = 0
            };
            var limitException = await Assert.ThrowsExceptionAsync<ArgumentException>(
                async () => await service
                    .GetDocumentGroupsAsync(options)
                    .ConfigureAwait(false)
                ).ConfigureAwait(false);

            StringAssert.Contains(limitException.Message, "Limit must be greater than 0 but less than or equal to 50.");
            Assert.AreEqual("options", limitException.ParamName);

            options.Limit = 2;
            options.Offset = -1;

            var offsetException = await Assert.ThrowsExceptionAsync<ArgumentException>(
                async () => await service
                    .GetDocumentGroupsAsync(options)
                    .ConfigureAwait(false)
            ).ConfigureAwait(false);

            StringAssert.Contains(offsetException.Message, "Offset must be 0 or greater.");
            Assert.AreEqual("options", offsetException.ParamName);
        }

        [TestMethod]
        public async Task ThrowsExceptionForWrongParamsClassTest()
        {
            var service = new DocumentGroupService(ApiBaseUrl, new Token(), SignNowClientMock("{}"));

            var instanceException = await Assert.ThrowsExceptionAsync<ArgumentException>(
                async () => await service
                    .GetDocumentGroupsAsync(new PagePaginationOptions())
                    .ConfigureAwait(false)
            ).ConfigureAwait(false);

            StringAssert.Contains(instanceException.Message, "Query params does not have 'limit' and 'offset' options. Use \"LimitOffsetOptions\" class.");
            Assert.AreEqual("options", instanceException.ParamName);
        }
    }
}
