using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using SignNow.Net.Model;
using SignNow.Net.Model.Requests.DocumentGroup;
using SignNow.Net.Model.Responses;
using SignNow.Net.Service;
using UnitTests;

namespace UnitTests
{
    [TestClass]
    public class CreateDocumentGroupFromTemplateTest : SignNowTestBase
    {
        private const string TemplateGroupId = "a1b2c3d4e5f6a1b2c3d4e5f6a1b2c3d4a1b2c3d4";
        private const string DocumentGroupId = "b1b2c3d4e5f6a1b2c3d4e5f6a1b2c3d4a1b2c3d4";

        [TestMethod]
        public async Task CreateDocumentGroupFromTemplateAsync_Success()
        {
            var expectedResponse = new DocumentGroupInfoResponse
            {
                Data = new DocumentGroupData
                {
                    Id = DocumentGroupId,
                    Name = "Group from template",
                    State = "active"
                }
            };

            var mockClient = SignNowClientMock(JsonConvert.SerializeObject(expectedResponse));
            var service = new DocumentGroupService(ApiBaseUrl, new Token(), mockClient);
            var request = new CreateDocumentGroupFromTemplateRequest
            {
                GroupName = "Group from template"
            };

            var result = await service.CreateDocumentGroupFromTemplateAsync(TemplateGroupId, request, CancellationToken.None);

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Data);
            Assert.AreEqual(DocumentGroupId, result.Data.Id);
            Assert.AreEqual("Group from template", result.Data.Name);
        }
    }
}
