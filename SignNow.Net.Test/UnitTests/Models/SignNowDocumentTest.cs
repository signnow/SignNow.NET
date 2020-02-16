using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using SignNow.Net.Model;
using SignNow.Net.Test;
using SignNow.Net.Test.FakeModels;

namespace UnitTests
{
    [TestClass]
    public class SignNowDocumentTest : SignNowTestBase
    {
        [TestMethod]
        public void ShouldDeserializeFromJson()
        {
            var fakeDocument = new SignNowDocumentFaker()
                .RuleFor(obj => obj.Roles, new RoleFaker().Generate(3))
                .RuleFor(obj => obj.Signatures, new SignatureFaker().Generate(3))
                .RuleFor(obj => obj.InviteRequests, new FreeformInviteFaker().Generate(2));

            var fakeDocumentJson = JsonConvert.SerializeObject(fakeDocument.Generate(), Formatting.Indented);

            var testDocument = JsonConvert.DeserializeObject<SignNowDocument>(fakeDocumentJson);
            var testDocumentJson = JsonConvert.SerializeObject(testDocument, Formatting.Indented);

            Assert.AreEqual(fakeDocumentJson, testDocumentJson);
        }
    }
}
