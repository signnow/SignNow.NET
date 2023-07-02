using System.IO;
using System.Net.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net.Internal.Requests;
using SignNow.Net.Model.ComplexTextTags;

namespace UnitTests
{
    [TestClass]
    public class MultipartHttpContentTest : SignNowTestBase
    {
        [TestMethod]
        public void CanGetHttpContent()
        {
            var testTag = new TextTag() {TagName = "Test"};
            using var testStream = new StreamContent(File.OpenRead(PdfFilePath));
            var multipart = new MultipartHttpContent(testStream.ReadAsStreamAsync().Result, "testFileName", testTag);

            var httpContent = multipart.GetHttpContent();
            TestUtils.Dump(httpContent);
            Assert.AreEqual(@"{""tag_name"":""Test""}", testTag.ToStringContent());
            StringAssert.StartsWith(httpContent.Headers.ContentType.ToString(), "multipart/form-data");
            // Assert.AreEqual("testFileName", httpContent.Headers.ContentDisposition);
        }
    }
}
