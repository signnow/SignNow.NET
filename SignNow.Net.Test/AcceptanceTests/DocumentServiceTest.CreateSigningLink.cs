using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Globalization;
using SignNow.Net.Exceptions;

namespace AcceptanceTests
{
    public partial class DocumentServiceTest
    {
        [TestMethod]
        public void SigningLinkCreatedSuccessfully()
        {
            var signingLinks = SignNowTestContext.Documents.CreateSigningLinkAsync(TestPdfDocumentIdWithFields).Result;

            Assert.IsNotNull(signingLinks.Url);
            Assert.IsNotNull(signingLinks.AnonymousUrl);
        }

        [TestMethod]
        public void SigningLinkExceptionIsCorrect()
        {
            var exception = Assert
                .ThrowsException<AggregateException>(
                    () => SignNowTestContext.Documents.CreateSigningLinkAsync("Some Wrong Document Id").Result);

            var expected = string
                .Format(CultureInfo.CurrentCulture, ExceptionMessages.InvalidFormatOfId, "Some Wrong Document Id");

            StringAssert.Contains(exception.Message, expected);
            StringAssert.Contains(exception.InnerException?.Message, expected);
        }
    }
}
