using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net.Model;
using SignNow.Net.Service;
using SignNow.Net.Test;

namespace AcceptanceTests
{
    [TestClass]
    public class UserServiceTest : AuthorizedApiTestBase
    {
        /// <summary>
        /// Test UserService instance
        /// </summary>
        private UserService userService;

        /// <summary>
        /// Test Document
        /// </summary>
        private string documentId;

        private readonly string emailPattern = @"(?<userid>\S+)@(?<domain>\w+.\w+)";
        private readonly string inviteIdPattern = @"^[a-zA-Z0-9_]{40,40}$";

        [TestInitialize]
        public void TestInitialize()
        {
            userService = new UserService(Token);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            DeleteDocument(documentId);
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
            var invite = new FreeFormInvite("signnow.tutorial+test@gmail.com");
            var inviteResponse = ProcessCreateInvite(invite);

            StringAssert.Matches(invite.Recipient, new Regex(emailPattern));
            StringAssert.Matches(inviteResponse.Id, new Regex(inviteIdPattern));
        }

        [TestMethod]
        public void ShouldCancelFreeformInvite()
        {
            var invite = new FreeFormInvite("signnow.tutorial+test@gmail.com");
            var inviteResponse = ProcessCreateInvite(invite);

            StringAssert.Matches(inviteResponse.Id, new Regex(inviteIdPattern));

            var cancelResponse = userService.CancelInviteAsync(inviteResponse.Id);
            Task.WaitAll(cancelResponse);

            Assert.IsFalse(cancelResponse.IsFaulted);
        }

        private InviteResponse ProcessCreateInvite(FreeFormInvite invite)
        {
            var documentService = new DocumentService(Token);
            documentId = UploadTestDocument(PdfFilePath, documentService);

            return userService.CreateInviteAsync(documentId, invite).Result;
        }

        [TestMethod]
        public void ThrowsExceptionForNullableInvite()
        {
            var actual = Assert.ThrowsException<AggregateException>(
                () => userService.CreateInviteAsync("", null).Result);

            var expected = "Value cannot be null."
                           + Environment.NewLine
                           + "Parameter name: invite";

            if (actual.InnerException != null)
            {
                Assert.AreEqual(expected, actual.InnerException.Message);
            }
        }
    }
}
