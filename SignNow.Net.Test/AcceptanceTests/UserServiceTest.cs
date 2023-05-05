using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net.Model;
using SignNow.Net.Test.Constants;
using UnitTests;

namespace AcceptanceTests
{
    [TestClass]
    public class UserServiceTest : AuthorizedApiTestBase
    {
        private readonly string emailPattern = @"(?<userid>\S+)@(?<domain>\w+.\w+)";
        private readonly string inviteIdPattern = @"^[a-zA-Z0-9_]{40,40}$";

        [TestMethod]
        public async Task ShouldGetUserInfo()
        {
            var userResponse = await SignNowTestContext.Users.GetCurrentUserAsync().ConfigureAwait(false);

            StringAssert.Matches(userResponse.Email, new Regex(emailPattern));
            Assert.IsTrue(userResponse.Active);
        }

        [TestMethod]
        public async Task ShouldCreateFreeformSignInvite()
        {
            var invite = new FreeFormSignInvite("signnow.tutorial+test@gmail.com")
            {
                Message = $"SignNow.Net SDK invited you to sign the {PdfFileName}",
                Subject = "SignNow.Net SDK Needs Your Signature"
            };
            var inviteResponse = await SignNowTestContext.Invites.CreateInviteAsync(TestPdfDocumentId, invite).ConfigureAwait(false);

            StringAssert.Matches(invite.Recipient, new Regex(emailPattern));
            StringAssert.Matches(inviteResponse.Id, new Regex(inviteIdPattern));
        }

        [TestMethod]
        public async Task ShouldCancelFreeformInvite()
        {
            var invite = new FreeFormSignInvite("signnow.tutorial+test@gmail.com");
            var inviteResponse = await SignNowTestContext.Invites.CreateInviteAsync(TestPdfDocumentId, invite).ConfigureAwait(false);

            StringAssert.Matches(inviteResponse.Id, new Regex(inviteIdPattern));

            var freeformInvite = new FreeformInvite
            {
                Id = inviteResponse.Id
            };

            var cancelResponse = SignNowTestContext.Invites.CancelInviteAsync(freeformInvite);
            Task.WaitAll(cancelResponse);

            Assert.IsFalse(cancelResponse.IsFaulted);
        }

        [TestMethod]
        public async Task ThrowsExceptionForNullableInvite()
        {
            var actual = await Assert.ThrowsExceptionAsync<ArgumentNullException>(
                async () => await SignNowTestContext.Invites
                    .CreateInviteAsync("", (SignInvite) null).ConfigureAwait(false))
                .ConfigureAwait(false);

            StringAssert.Contains(actual.Message, ErrorMessages.ValueCannotBeNull);
            StringAssert.Contains(actual.ParamName, "invite");
        }
    }
}
