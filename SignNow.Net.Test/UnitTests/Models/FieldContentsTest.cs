using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net.Interfaces;
using SignNow.Net.Model.FieldContents;
using SignNow.Net.Test.FakeModels;

namespace UnitTests
{
    [TestClass]
    public class FieldContentsTest
    {
        [DataTestMethod]
        [DynamicData(nameof(FieldContentProvider), DynamicDataSourceType.Method, DynamicDataDisplayName = nameof(FieldContentNameProvider))]
        public void ShouldGetFieldContentValue(string testName, ISignNowContent fieldContent)
        {
            Assert.IsNotNull(fieldContent?.GetValue());
            Assert.IsNotNull(fieldContent?.GetValue().ToString());
        }

        public static IEnumerable<object[]> FieldContentProvider()
        {
            // Test DisplayName | test object
            yield return new object[] { nameof(RadiobuttonContent), new RadiobuttonContentFaker().Generate() };
            yield return new object[] { nameof(EnumerationContent), new EnumerationContentFaker().Generate() };
            yield return new object[] { nameof(AttachmentContent), new AttachmentContentFaker().Generate() };
            yield return new object[] { nameof(HyperlinkContent), new HyperlinkContentFaker().Generate() };
            yield return new object[] { nameof(SignatureContent), new SignatureContentFaker().Generate() };
            yield return new object[] { nameof(CheckboxContent), new CheckboxContentFaker().Generate() };
            yield return new object[] { nameof(RadioContent), new RadioContentFaker().Generate() };
            yield return new object[] { nameof(TextContent), new TextContentFaker().Generate() };
        }

        public static string FieldContentNameProvider(MethodInfo methodInfo, object[] data)
        {
            return data.First().ToString();
        }
    }
}
