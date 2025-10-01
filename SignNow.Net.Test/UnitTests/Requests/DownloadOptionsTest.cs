using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net.Model.Requests.DocumentGroup;
using UnitTests;

namespace UnitTests.Requests
{
    [TestClass]
    public class DownloadOptionsTest
    {
        [TestMethod]
        public void DownloadOptionsWithDefaultsTest()
        {
            var downloadOptions = new DownloadOptions();

            Assert.AreEqual(DownloadType.Zip, downloadOptions.DownloadType);
            Assert.AreEqual(DocumentHistoryType.NoHistory, downloadOptions.WithHistory);
            Assert.AreEqual(0, downloadOptions.DocumentOrder.Count);
        }

        [TestMethod]
        public void Serialize_ShouldIncludeAllPropertiesTest()
        {
            var options = new DownloadOptions
            {
                DownloadType = DownloadType.PdfWithCertificate,
                WithHistory = DocumentHistoryType.AfterEachDocument,
                DocumentOrder = new List<string> { "03c74b3083f34ebf8ef40a3039dfb32c85a08437", "03739a736d324f9794c2e93ec7c5bda817af3f7f" }
            };

            var expectedJson = "{\"type\":\"certificate\",\"with_history\":\"after_each_document\",\"document_order\":[\"03c74b3083f34ebf8ef40a3039dfb32c85a08437\",\"03739a736d324f9794c2e93ec7c5bda817af3f7f\"]}";

            Assert.That.JsonEqual(options, expectedJson);
        }

        [TestMethod]
        public void Serialize_ShouldIgnoreNullDocumentOrderTest()
        {
            var options = new DownloadOptions
            {
                DownloadType = DownloadType.ZipForEmail,
                WithHistory = DocumentHistoryType.AfterMergedPdf,
                DocumentOrder = null
            };

            var expectedJson = "{\"type\":\"email\",\"with_history\":\"after_merged_pdf\"}";

            Assert.That.JsonEqual(options, expectedJson);
        }

        [TestMethod]
        public void Deserialize_ShouldSetAllPropertiesTest()
        {
            var json = "{\"type\":\"merged\",\"with_history\":\"no\",\"document_order\":[\"03c74b3083f34ebf8ef40a3039dfb32c85a08437\",\"03c74b3083f34ebf8ef40a3039dfb32c85a08438\"]}";
            var options = TestUtils.DeserializeFromJson<DownloadOptions>(json);

            Assert.AreEqual(DownloadType.MergedPdf, options.DownloadType);
            Assert.AreEqual(DocumentHistoryType.NoHistory, options.WithHistory);
            CollectionAssert.AreEqual(new List<string> { "03c74b3083f34ebf8ef40a3039dfb32c85a08437", "03c74b3083f34ebf8ef40a3039dfb32c85a08438" }, options.DocumentOrder);
        }

    }
}
