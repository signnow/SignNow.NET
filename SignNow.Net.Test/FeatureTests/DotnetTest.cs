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
#pragma warning disable IDE0028 // Simplify collection initialization
#pragma warning disable CA2000 // Dispose objects before losing scope
#pragma warning disable CA1031 // Do not catch general exception types

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
#pragma warning restore CA2000 // Dispose objects before losing scope
#pragma warning restore IDE0028 // Simplify collection initialization
#pragma warning restore CA1031 // Do not catch general exception types

    }
}
