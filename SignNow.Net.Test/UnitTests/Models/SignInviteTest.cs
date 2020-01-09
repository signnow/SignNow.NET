using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SignNow.Net.Model;

namespace UnitTests
{
    [TestClass]
    public class SignInviteTest
    {
        [DataTestMethod]
        [DataRow("This is Email body message content", "This is Email subject content", DisplayName = "with custom message and subject")]
        [DataRow(default, default, DisplayName = "without message and subject")]
        public void ShouldCreateFreeformInviteContent(string message, string subject)
        {
            var recipient = "recipient@signnow.com";

            var invite = new FreeFormInvite(recipient)
            {
                Message = message,
                Subject = subject
            };

            var sender = new User
            {
                Active = true,
                Email = "sender@signnow.com",
                FirstName = "Unit",
                LastName = "Test"
            };

            var requestInvite = JObject.FromObject(invite);

            requestInvite.Add("from", sender.Email);

            var jsonSubject = null == subject ? "null" : $"\"{subject}\"";
            var jsonMessage = null == message ? "null" : $"\"{message}\"";

            var expected = $"{{\"to\":\"recipient@signnow.com\",\"subject\":{jsonSubject},\"message\":{jsonMessage},\"from\":\"sender@signnow.com\"}}";

            Assert.AreEqual(expected, JsonConvert.SerializeObject(requestInvite, Formatting.None));
        }
    }
}
