using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net.Internal.Requests;
using SignNow.Net.Test.FakeModels;

namespace UnitTests.Requests
{
    [TestClass]
    public class MergeDocumentRequestTest : SignNowTestBase
    {
        [TestMethod]
        public void SerializeModel()
        {
            const string MergedName = "Test_Merged_Document_Name";
            var documents = new SignNowDocumentFaker().Generate(5);

            var request = new MergeDocumentRequest
            {
                Name = MergedName
            };
            request.AddDocuments(documents);

            var serialized = TestUtils.SerializeToJsonFormatted(request);
            var requestJ = TestUtils.DeserializeFromJson<MergeDocumentRequest>(serialized);

            Assert.AreEqual(MergedName, requestJ.Name);
            Assert.AreEqual(5, requestJ.DocumentIds.Count);
            Assert.AreEqual(documents[1].Id, requestJ.DocumentIds[1]);
        }

        [TestMethod]
        public void CreatesJsonContext()
        {
            var documents = new SignNowDocumentFaker().Generate(10);
            var request = new MergeDocumentRequest
            {
                Name = "mergedName"
            };
            request.AddDocuments(documents);

            var jsonHttpContext = request.GetHttpContent();
            var jsonBody = jsonHttpContext.ReadAsStringAsync().Result;

            Assert.AreEqual("application/json", jsonHttpContext.Headers.ContentType?.MediaType);
            Assert.IsTrue(jsonBody.Contains("mergedName"));
            Assert.IsTrue(jsonBody.Contains(documents[1].Id));
        }
    }
}
