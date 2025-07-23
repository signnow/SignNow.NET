using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net.Interfaces;
using SignNow.Net.Test.FakeModels;

namespace UnitTests.Models
{
    [TestClass]
    public class FieldContentsTest : SignNowTestBase
    {
        [TestMethod]
        public void GetValueOfFieldContent()
        {
            var fields = new List<ISignNowContent>()
            {
                new RadiobuttonContentFaker().Generate(),
                new EnumerationContentFaker().Generate(),
                new AttachmentContentFaker().Generate(),
                new HyperlinkContentFaker().Generate(),
                new SignatureContentFaker().Generate(),
                new CheckboxContentFaker().Generate(),
                new RadioContentFaker().Generate(),
                new TextContentFaker().Generate()
            };

            foreach (var fieldContent in fields)
            {
                Assert.IsNotNull(fieldContent?.GetValue(), nameof(fieldContent) + " is empty\n" + TestUtils.SerializeToJsonFormatted(fieldContent));
                Assert.IsNotNull(fieldContent?.GetValue().ToString());
            }
        }
    }
}
