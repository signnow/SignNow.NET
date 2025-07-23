using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Globalization;
using System.Threading.Tasks;
using SignNow.Net.Exceptions;

namespace AcceptanceTests
{
    public partial class DocumentServiceTest
    {
        [TestMethod]
        public async Task SigningLinkCreatedSuccessfully()
        {
            var signingLinks = await SignNowTestContext.Documents
                .CreateSigningLinkAsync(TestPdfDocumentIdWithFields)
                .ConfigureAwait(false);

            Assert.IsNotNull(signingLinks.Url);
            Assert.IsNotNull(signingLinks.AnonymousUrl);
        }

        [TestMethod]
        public async Task ThrowsExceptionForWrongDocumentId()
        {
            var exception = await Assert
                .ThrowsExceptionAsync<ArgumentException>(
                    async () => await SignNowTestContext.Documents
                        .CreateSigningLinkAsync("Some Wrong Document Id")
                        .ConfigureAwait(false))
                .ConfigureAwait(false);

            var expected = string
                .Format(CultureInfo.CurrentCulture, ExceptionMessages.InvalidFormatOfId, "Some Wrong Document Id");

            StringAssert.Contains(exception.Message, expected);
            Assert.AreEqual("Some Wrong Document Id", exception.ParamName);
        }
    }
}
