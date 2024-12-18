using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net.Internal.Helpers.Converters;
using SignNow.Net.Model.Requests.DocumentGroup;

namespace UnitTests.Requests
{
    [TestClass]
    public class CopyDocumentGroupRequestTest
    {
        [TestMethod]
        public void ShouldBeSerializedAsWithDefaultTimestampTest()
        {
            var request = new CopyDocumentGroupRequest
            {
                DocumentGroupName = "test name"
            };
            var actualTime = UnixTimeStampConverter.ToUnixTimestamp(request.ClientTimestamp);

            var expectedJson = $@"{{""document_group_name"":""test name"",""client_timestamp"":""{actualTime}""}}";

            Assert.That.JsonEqual(expectedJson, request);
        }
    }
}
