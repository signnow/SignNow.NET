using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net.Interfaces;
using SignNow.Net.Test.FakeModels;

namespace UnitTests
{
    [TestClass]
    public class FieldContentsTest
    {
        [TestMethod]
        public void ShouldGetFieldContentValue()
        {
            var contents = new List<ISignNowContent>();

            contents.Add(new TextContentFaker().Generate());
            contents.Add(new SignatureContentFaker().Generate());
            contents.Add(new RadiobuttonContentFaker().Generate());
            contents.Add(new HyperlinkContentFaker().Generate());
            contents.Add(new EnumerationContentFaker().Generate());
            contents.Add(new CheckboxContentFaker().Generate());
            contents.Add(new AttachmentContentFaker().Generate());
            contents.Add(new RadioContentFaker().Generate());

            foreach (var element in contents)
            {
                Assert.IsNotNull(element.GetValue());
                Assert.IsNotNull(element.GetValue().ToString());
            }
        }
    }
}
