using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SignNow.Net.Model;

namespace UnitTests
{
    [TestClass]
    public partial class SignInviteTest
    {
        private readonly string senderEmail = "sender@signnow.com";
        private readonly string recipientEmail = "recipient@signnow.com";

        [DataTestMethod]
        [DataRow("This is Email body message content", "This is Email subject content", DisplayName = "with custom message and subject")]
        [DataRow(default, default, DisplayName = "without message and subject")]
        public void ShouldCreateFreeformInviteContent(string message, string subject)
        {
            var invite = new FreeFormSignInvite(recipientEmail)
            {
                Message = message,
                Subject = subject
            };

            var sender = new User
            {
                Active = true,
                Email = senderEmail,
                FirstName = "Unit",
                LastName = "Test"
            };

            var requestInvite = JObject.FromObject(invite);

            requestInvite.Add("from", sender.Email);

            var jsonSubject = null == subject ? "null" : $"\"{subject}\"";
            var jsonMessage = null == message ? "null" : $"\"{message}\"";

            var expected = $"{{\"to\":\"{recipientEmail}\",\"subject\":{jsonSubject},\"message\":{jsonMessage},\"from\":\"{senderEmail}\"}}";

            Assert.AreEqual(expected, JsonConvert.SerializeObject(requestInvite, Formatting.None));
        }

        [TestMethod]
        public void ShouldSerializeFreeFormInvite()
        {
            var ffInvite = new FreeFormSignInvite(recipientEmail);
            var expected = $"{{\"to\":\"{recipientEmail}\",\"subject\":null,\"message\":null}}";

            Assert.AreEqual(expected, JsonConvert.SerializeObject(ffInvite));
        }
    }
}
