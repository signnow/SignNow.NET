using System.IO;
using System.Net.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net.Internal.Requests;
using SignNow.Net.Model;
using SignNow.Net.Model.ComplexTags;

namespace UnitTests
{
    [TestClass]
    public class MultipartHttpContentTest : SignNowTestBase
    {
        [TestMethod]
        public void CanGetHttpContent()
        {
            var testTag = new TextTag()
            {
                TagName = "Test",
                Role = "Signer 1",
                Required = true,
                Label = "name",
                PrefilledText = "enter-the-name",
                Width = 100,
                Height = 15
            };
            var complexTags = new ComplexTextTags(testTag);

            using var testStream = new StreamContent(File.OpenRead(PdfFilePath));
            var multipart = new MultipartHttpContent(testStream.ReadAsStreamAsync().Result, "testFileName", complexTags);

            var httpContent = multipart.GetHttpContent();
            var requestContent = httpContent.ReadAsStringAsync().Result;

            StringAssert.StartsWith(httpContent.Headers.ContentType.ToString(), "multipart/form-data");
            StringAssert.Contains(
                requestContent,
                "Content-Disposition: form-data; name=file; filename=testFileName; filename*=utf-8''testFileName");
            StringAssert.Contains(
                requestContent,
                "Content-Type: text/plain; charset=utf-8\r\nContent-Disposition: form-data; name=Tags");
            StringAssert.Contains(
                requestContent,
                "[{\"type\":\"text\",\"prefilled_text\":\"enter-the-name\",\"label\":\"name\",\"tag_name\":\"Test\",\"role\":\"Signer 1\",\"required\":true,\"width\":100,\"height\":15}]");
        }
    }
}
