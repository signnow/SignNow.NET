using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using SignNow.Net.Exceptions;
using SignNow.Net.Model;
using SignNow.Net.Test.FakeModels;
using UnitTests;

namespace UnitTests.Models
{
    [TestClass]
    public partial class SignInviteTest : SignNowTestBase
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

            var expected = $"{{\"to\":\"{recipientEmail}\",\"subject\":{jsonSubject},\"message\":{jsonMessage},\"cc\":[],\"from\":\"{senderEmail}\"}}";

            Assert.That.JsonEqual(expected, requestInvite);
        }

        [TestMethod]
        public void ShouldSerializeFreeFormInvite()
        {
            var ffInvite = new FreeFormSignInvite(recipientEmail);
            var expected = $"{{\"to\":\"{recipientEmail}\",\"subject\":null,\"message\":null,\"cc\":[]}}";

            Assert.That.JsonEqual(expected, ffInvite);
        }

        [TestMethod]
        public void ShouldProperSerializeCcField()
        {
            var invite = new FreeFormSignInvite(recipientEmail);
            var inviteCc = new FreeFormSignInvite(recipientEmail, "user1@email.com");

            var expectedEmptyCc = $"{{\"to\":\"{recipientEmail}\",\"subject\":null,\"message\":null,\"cc\":[]}}";
            var expectedFilledCc = $"{{\"to\":\"{recipientEmail}\",\"subject\":null,\"message\":null,\"cc\":[\"user1@email.com\"]}}";

            Assert.That.JsonEqual(expectedEmptyCc, invite);
            Assert.That.JsonEqual(expectedFilledCc, inviteCc);
        }

        [TestMethod]
        public void ShouldAddOnlyUniqEmailsToCcList()
        {
            var receivers = new List<string>
            {
                "user1@email.com",
                "user1@email.com",
                "user2@email.com",
                "user2@email.com",
                "user3@email.com",
                "user3@email.com"
            };

            var inviteCtor1 = new FreeFormSignInvite(recipientEmail, "user1@email.com");
            var inviteCtor2 = new FreeFormSignInvite(recipientEmail, receivers);

            inviteCtor1.AddCcRecipients(receivers);
            inviteCtor2.AddCcRecipients("user1@email.com");

            Assert.AreEqual(3, inviteCtor1.Cc.Count());
            Assert.AreEqual(3, inviteCtor2.Cc.Count());
        }


        [TestMethod]
        public void ThrowsExceptionForErroredEmails()
        {
            var exception = Assert
                .ThrowsException<ArgumentException>(
                    () => new FreeFormSignInvite(recipientEmail, "not-valid-email.com"));

            var errorMessage = string
                .Format(CultureInfo.CurrentCulture, ExceptionMessages.InvalidFormatOfEmail, "not-valid-email.com");

            StringAssert.Contains(exception.Message, errorMessage);
        }

        [TestMethod]
        public void ThrowsExceptionWhenDocumentDoesNotHaveRoles()
        {
            var document = new SignNowDocumentFaker().Generate();

            var exception = Assert.ThrowsException<ArgumentException>(
                () => new EmbeddedSigningInvite(document));

            Assert.AreEqual("This document does not contain Roles.", exception.Message);
        }

        [TestMethod]
        public void ThrowsExceptionWhenInviteIsAlreadyExistsForDocument()
        {
            var document = new SignNowDocumentFaker()
                .RuleFor(o => o.Roles, new RoleFaker().Generate(2))
                .RuleFor(o => o.FieldInvites, new FieldInviteFaker().Generate(2))
                .Generate();

            var exception = Assert.ThrowsException<ArgumentException>(
                () => new EmbeddedSigningInvite(document));

            Assert.AreEqual("An invite already exists for this document.", exception.Message);
        }

        [TestMethod]
        public void ThrowsExceptionWhenRoleIdDoesNotExists()
        {
            var document = new SignNowDocumentFaker()
                .RuleFor(o => o.Roles, new RoleFaker().Generate(2))
                .Generate();

            var invite = new EmbeddedSigningInvite(document);

            var exception = Assert.ThrowsException<ArgumentException>(
                () => invite.AddEmbeddedSigningInvite(new EmbeddedInvite() {RoleId = "test"}));

            Assert.AreEqual("options", exception.ParamName);
            Assert.IsTrue(exception.Message.StartsWith("RoleId does not exists"));
        }
    }
}
