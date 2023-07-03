using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net.Model.ComplexTags;

namespace UnitTests.Models.ComplexTags
{
    [TestClass]
    public class ComplexTagsTest
    {
        [TestMethod]
        public void ShouldSerializeAttachmentsTag()
        {
            var tag = new AttachmentTag
            {
                Height = 100,
                Width = 200,
                Label = "label_name",
                Required = true,
                Role = "Role_1",
                TagName = "attached document"
            };

            var expected = @"{
                ""type"": ""attachments"",
                ""label"": ""label_name"",
                ""tag_name"": ""attached document"",
                ""role"": ""Role_1"",
                ""required"": true,
                ""width"": 200,
                ""height"": 100
            }";

            Assert.That.JsonEqual(expected, tag);
        }

        [TestMethod]
        public void ShouldSerializeCheckBoxTag()
        {
            var tag = new CheckBoxTag
            {
                Height = 12,
                Width = 12,
                Required = true,
                Role = "CLIENT",
                TagName = "CheckboxTagExample"
            };

            var expected = @"{
                ""type"": ""checkbox"",
                ""tag_name"": ""CheckboxTagExample"",
                ""role"": ""CLIENT"",
                ""required"": true,
                ""width"": 12,
                ""height"": 12
            }";

            Assert.That.JsonEqual(expected, tag);
        }

        [TestMethod]
        public void ShouldSerializeDropdownTag()
        {
            var tag = new DropdownTag
            {
                TagName = "DropdownTagExample",
                Role = "CLIENT",
                Required = true,
                EnumerationOptions = new List<string> {"All", "None"},
                Width = 100,
                Height = 15,
            };

            var expected = @"{
                ""type"": ""enumeration"",
                ""custom_defined_option"": false,
                ""enumeration_options"": [
                    ""All"",
                    ""None""
                  ],
                ""tag_name"": ""DropdownTagExample"",
                ""role"": ""CLIENT"",
                ""required"": true,
                ""width"": 100,
                ""height"": 15
            }";

            Assert.That.JsonEqual(expected, tag);
        }

        [TestMethod]
        public void ShouldSerializeHyperlinkTag()
        {
            var tag = new HyperlinkTag
            {
                TagName = "hyperlink_example",
                PageNumber = 0,
                Role = "CLIENT",
                Name = "HyperlinkTagExampleUID",
                Required = true,
                Width = 100,
                Height = 15,
                Link = new Uri("https://signnow.com"),
                Label = "signNow main page",
                Hint = "click"
            };

            var expected = @"{
                ""type"": ""hyperlink"",
                ""page_number"": 0,
                ""name"": ""HyperlinkTagExampleUID"",
                ""link"": ""https://signnow.com"",
                ""hint"": ""click"",
                ""label"": ""signNow main page"",
                ""tag_name"": ""hyperlink_example"",
                ""role"": ""CLIENT"",
                ""required"": true,
                ""width"": 100,
                ""height"": 15
            }";

            Assert.That.JsonEqual(expected, tag);
        }

        [TestMethod]
        public void ShouldSerializeInitialsTag()
        {
            var tag = new InitialsTag
            {
                TagName = "InitialsTagExample",
                Role = "CLIENT",
                Required = true,
                Height = 15,
                Width = 40,
            };

            var expected = @"{
                ""type"": ""initials"",
                ""tag_name"": ""InitialsTagExample"",
                ""role"": ""CLIENT"",
                ""required"": true,
                ""width"": 40,
                ""height"": 15
            }";

            Assert.That.JsonEqual(expected, tag);
        }

        [TestMethod]
        public void ShouldSerializeSignatureTag()
        {
            var tag = new SignatureTag
            {
                TagName = "SignatureTagExample",
                Role = "CLIENT",
                Required = true,
                Height = 15,
                Width = 400,
            };

            var expected = @"{
                ""type"": ""signature"",
                ""tag_name"": ""SignatureTagExample"",
                ""role"": ""CLIENT"",
                ""required"": true,
                ""width"": 400,
                ""height"": 15
            }";

            Assert.That.JsonEqual(expected, tag);
        }

        [TestMethod]
        public void ShouldSerializeTextTag()
        {
            var tag = new TextTag
            {
                TagName = "TextTagExample",
                Role = "CLIENT",
                Label = "Label1",
                Required = true,
                Height = 150,
                Width = 400,
            };

            var expected = @"{
                ""type"": ""text"",
                ""label"": ""Label1"",
                ""tag_name"": ""TextTagExample"",
                ""role"": ""CLIENT"",
                ""required"": true,
                ""width"": 400,
                ""height"": 150
            }";

            Assert.That.JsonEqual(expected, tag);
        }
    }
}
