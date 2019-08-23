using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Text;

namespace FeatureTests

{
    [TestClass]
    public partial class DotnetTest
    {
        [TestMethod]
        public void StreamContentIsDisposedInMultipartFormDataContent()
        {
            
            var streamContent = new MemoryStream(Encoding.UTF8.GetBytes("Hello world!"));
            var multipartContent = new MultipartFormDataContent("Upload----" + DateTime.Now.ToString(CultureInfo.InvariantCulture));
#pragma warning disable CA2000 // Dispose objects before losing scope
            multipartContent.Add(new StreamContent(streamContent), "file", "upload.pdf");
#pragma warning restore CA2000 // Dispose objects before losing scope
            HttpContent content = multipartContent;
            content.Dispose();
            try
            {
                var result = multipartContent.ReadAsStringAsync().Result;
                Assert.Fail();
            }
            catch (ObjectDisposedException)
            {
            }
            try
            {
                var result = streamContent.Length;
                Assert.Fail("Underline stream content is not disposed!");
            }
            catch (ObjectDisposedException)
            {
            }
        }
    }
}
