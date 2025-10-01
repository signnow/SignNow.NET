using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Text;

namespace FeatureTests
{
    [TestClass]
    public class DotnetTest
    {
        [TestMethod]
        public void StreamContentIsDisposedInMultipartFormDataContent()
        {
            var streamContent = new MemoryStream(Encoding.UTF8.GetBytes("Hello world!"));
            var multipartContent = new MultipartFormDataContent("Upload----" + DateTime.Now.ToString(CultureInfo.InvariantCulture));
            multipartContent.Add(new StreamContent(streamContent), "file", "upload.pdf");

            HttpContent content = multipartContent;
            content.Dispose();

            try
            {
                var result = multipartContent.ReadAsStringAsync().Result;
                Assert.Fail(result);
            }
            catch (ObjectDisposedException)
            {
            }
            try
            {
                var result = streamContent.Length;
                Assert.Fail("Underline stream content is not disposed! Content length = " + result.ToString(CultureInfo.InvariantCulture));
            }
            catch (ObjectDisposedException)
            {
            }
        }
    }
}
