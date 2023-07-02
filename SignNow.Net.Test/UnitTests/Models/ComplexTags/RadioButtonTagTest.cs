using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net.Model.ComplexTags;

namespace UnitTests.Models.ComplexTags
{
    [TestClass]
    public class RadioButtonTagTest
    {
        [TestMethod]
        public void ShouldProperSerialize()
        {
            var radioButton = new RadioButtonTag("Group_name")
            {
                TagName = "radio_group_1",
                PageNumber = 0,
                Role = "Signer 1",
                Required = true,
                X = 36,
                Y = 246,
                Width = 340,
                Height = 13,
            };

            radioButton.AddOption("value-1", 0, 0);
            radioButton.AddOption("value-2", 0, 14, 1);
            radioButton.AddOption("value-3", 0, 28);

            var actual = @"{
                ""type"":""radiobutton"",
                ""name"":""Group_name"",
                ""page_number"": 0,
                ""x"": 36,
                ""y"": 246,
                ""radio"": [
                    {
                        ""page_number"": 0,
                        ""x"": 36,
                        ""y"": 246,
                        ""width"": 13,
                        ""height"": 13,
                        ""value"": ""value-1"",
                        ""checked"": 0,
                        ""x-offset"": 0,
                        ""y-offset"": 0
                    },
                    {
                        ""page_number"": 0,
                        ""x"": 36,
                        ""y"": 246,
                        ""width"": 13,
                        ""height"": 13,
                        ""value"": ""value-2"",
                        ""checked"": 1,
                        ""x-offset"": 0,
                        ""y-offset"": 14
                    },
                    {
                        ""page_number"": 0,
                        ""x"": 36,
                        ""y"": 246,
                        ""width"": 13,
                        ""height"": 13,
                        ""value"": ""value-3"",
                        ""checked"": 0,
                        ""x-offset"": 0,
                        ""y-offset"": 28
                    }
                ],
                ""tag_name"": ""radio_group_1"",
                ""role"": ""Signer 1"",
                ""required"": true,
                ""width"": 340,
                ""height"": 13
            }";

            Assert.That.JsonEqual(actual, radioButton);
        }
    }
}
