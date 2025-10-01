using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SignNow.Net.Examples
{
    public partial class DocumentExamples
    {
        [TestMethod]
        public async Task CreateSigningLinkToTheDocumentAsync()
        {
            // Upload a document with a signature field
            await using var fileStream = File.OpenRead(PdfWithSignatureField);
            var document = await testContext.Documents
                .UploadDocumentWithFieldExtractAsync(fileStream, "CreateSigningLinkToTheDocument.pdf")
                .ConfigureAwait(false);

            // Create a signing link to the document for signature
            var signingLink = await testContext.Documents
                .CreateSigningLinkAsync(document?.Id)
                .ConfigureAwait(false);

            // Validate the response
            Assert.IsNotNull(signingLink.Url);
            Assert.IsNotNull(signingLink.AnonymousUrl);
            Assert.IsInstanceOfType(signingLink.Url, typeof(Uri));
            Assert.AreEqual("https", signingLink.Url.Scheme);
            Assert.IsFalse(string.IsNullOrEmpty(signingLink.Url.OriginalString));

            // Clean up
            DeleteTestDocument(document?.Id);
        }
    }
}
