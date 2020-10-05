using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net.Internal.Requests;
using SignNow.Net.Test;
using SignNow.Net.Test.FakeModels;

namespace UnitTests
{
    [TestClass]
    public class MergeDocumentRequestTest : SignNowTestBase
    {
        [TestMethod]
        public void SerializeModel()
        {
            var mergedName = "Test_Merged_Document_Name";
            var documents = new SignNowDocumentFaker().Generate(5);

            var request = new MergeDocumentRequest
            {
                Name = mergedName
            };
            request.AddDocuments(documents);

            var serialized = TestUtils.SerializeToJsonFormatted(request);
            var requestJ = TestUtils.DeserializeFromJson<MergeDocumentRequest>(serialized);

            Assert.AreEqual(mergedName, requestJ.Name);
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

            var JsonHttpContext = request.GetHttpContent();
            var jsonBody = JsonHttpContext.ReadAsStringAsync().Result;

            Assert.AreEqual("application/json", JsonHttpContext.Headers.ContentType.MediaType);
            Assert.IsTrue(jsonBody.Contains("mergedName"));
            Assert.IsTrue(jsonBody.Contains(documents[1].Id));
        }
    }
}
