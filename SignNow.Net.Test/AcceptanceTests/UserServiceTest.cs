using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net.Model;
using SignNow.Net.Service;
using SignNow.Net.Test;
using SignNow.Net.Test.Constants;

namespace AcceptanceTests
{
    [TestClass]
    public class UserServiceTest : AuthorizedApiTestBase
    {
        /// <summary>
        /// Test UserService instance
        /// </summary>
        private UserService userService;

        private readonly string emailPattern = @"(?<userid>\S+)@(?<domain>\w+.\w+)";
        private readonly string inviteIdPattern = @"^[a-zA-Z0-9_]{40,40}$";

        /// <summary>
        /// Reusable method to upload test document and create invite request.
        /// </summary>
        /// <param name="invite">FreeForm invite object.</param>
        /// <returns>InviteResponse</returns>
        private InviteResponse ProcessCreateInvite(FreeFormSignInvite invite)
        {
            var documentService = new DocumentService(Token);
            DocumentId = UploadTestDocument(PdfFilePath, documentService);

            return userService.CreateInviteAsync(DocumentId, invite).Result;
        }

        [TestInitialize]
        public void Setup()
        {
            userService = new UserService(Token);
        }

        [TestMethod]
        public void ShouldGetUserInfo()
        {
            var userResponse = userService.GetCurrentUserAsync().Result;

            StringAssert.Matches(userResponse.Email, new Regex(emailPattern));
            Assert.IsTrue(userResponse.Active);
        }

        [TestMethod]
        public void ShouldCreateFreeformSignInvite()
        {
            var invite = new FreeFormSignInvite("signnow.tutorial+test@gmail.com")
            {
                Message = $"SignNow.Net SDK invited you to sign the {pdfFileName}",
                Subject = "SignNow.Net SDK Needs Your Signature"
            };
            var inviteResponse = ProcessCreateInvite(invite);

            StringAssert.Matches(invite.Recipient, new Regex(emailPattern));
            StringAssert.Matches(inviteResponse.Id, new Regex(inviteIdPattern));
        }

        [TestMethod]
        public void ShouldCancelFreeformInvite()
        {
            var invite = new FreeFormSignInvite("signnow.tutorial+test@gmail.com");
            var inviteResponse = ProcessCreateInvite(invite);

            StringAssert.Matches(inviteResponse.Id, new Regex(inviteIdPattern));

            var freeformInvite = new FreeformInvite
            {
                Id = inviteResponse.Id
            };

            var cancelResponse = userService.CancelInviteAsync(freeformInvite);
            Task.WaitAll(cancelResponse);

            Assert.IsFalse(cancelResponse.IsFaulted);
        }

        [TestMethod]
        public void ThrowsExceptionForNullableInvite()
        {
            var actual = Assert.ThrowsException<AggregateException>(
                () => userService.CreateInviteAsync("", null).Result);

            StringAssert.Contains(actual.InnerException?.Message, ErrorMessages.ValueCannotBeNull);
            StringAssert.Contains(actual.InnerException?.Message, "invite");
        }
    }
}
