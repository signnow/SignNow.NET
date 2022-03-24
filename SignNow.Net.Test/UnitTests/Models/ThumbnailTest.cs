using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using SignNow.Net.Model;
using SignNow.Net.Test.FakeModels;

namespace UnitTests
{
    [TestClass]
    public class ThumbnailTest
    {
        [TestMethod]
        public void ShouldDeserializeFromJson()
        {
            const string Json = @"{
                ""small"": ""https://api.signnow.com/document/a09b26feeba7ce70228afe6290f4445700b6f349/thumbnail?size=small"",
                ""medium"":""https://api.signnow.com/document/a09b26feeba7ce70228afe6290f4445700b6f349/thumbnail?size=medium"",
                ""large"": ""https://api.signnow.com/document/a09b26feeba7ce70228afe6290f4445700b6f349/thumbnail?size=large""
            }";

            var actual = JsonConvert.DeserializeObject<Thumbnail>(Json);

            Assert.AreEqual(
                "https://api.signnow.com/document/a09b26feeba7ce70228afe6290f4445700b6f349/thumbnail?size=small",
                actual.Small.AbsoluteUri);
            Assert.AreEqual("?size=small", actual.Small.Query);
            Assert.AreEqual("?size=medium", actual.Medium.Query);
            Assert.AreEqual("?size=large", actual.Large.Query);
        }

        [TestMethod]
        public void ShouldBeSerializable()
        {
            var model = new ThumbnailFaker().Generate();

            var actual = JsonConvert.SerializeObject(model);

            StringAssert.Contains(actual, "small");
            StringAssert.Contains(actual, "medium");
            StringAssert.Contains(actual, "large");
            StringAssert.Contains(actual, "https://via.placeholder.com");
        }
    }
}
