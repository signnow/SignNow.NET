using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
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
        private InviteResponse ProcessCreateInvite(FreeFormInvite invite)
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

            var freeformInvite = new FreeformInvite
            {
                Id = inviteResponse.Id
            };

            var cancelResponse = userService.CancelInviteAsync(freeformInvite);
            Task.WaitAll(cancelResponse);

            Assert.IsFalse(cancelResponse.IsFaulted);
        }

        [TestMethod]
        public void ShouldCreateRoleBasedInviteRequest()
        {
            var json = @"{
                'id': 'a09b26feeba7ce70228afe6290f4445700b6f349',
                'user_id': '890d13607d89a7b3f6e67a14757d02ec00cf5eae',
                'document_name': 'pdf-test',
                'page_count': '1',
                'created': '1565787561',
                'updated': '1565858757',
                'original_filename': 'pdf-test.pdf',
                'origin_user_id': null,
                'origin_document_id': null,
                'owner': 'test.dotnet@signnow.com',
                'template': false,
                'roles': [
                    {
                        'unique_id': '485a05488fb971644978d3ec943ff6c719bda83a',
                        'signing_order': '1',
                        'name': 'Signer 1'
                    }
                ],
                'requests': []
            }";

            var document = JsonConvert.DeserializeObject<SignNowDocument>(json);
            var invite = new RoleBasedInvite(document);

            Assert.AreEqual(1, invite.DocumentRoles().Count);
            Assert.AreEqual("Signer 1", invite.DocumentRoles().First().Name);

            Assert.AreEqual($"{{\"to\":[],\"subject\":null,\"message\":null}}", JsonConvert.SerializeObject(invite));

            invite.AddRoleBasedInvite(
                new SignerOptions("signer1@signnow.com", invite.DocumentRoles().First())
            );

            var invitee = $"{{\"email\":\"signer1@signnow.com\",\"role\":\"Signer 1\",\"role_id\":\"485a05488fb971644978d3ec943ff6c719bda83a\",\"order\":1}}";
            var expectedInvite = $"{{\"to\":[{invitee}],\"subject\":null,\"message\":null}}";

            Assert.AreEqual(expectedInvite, JsonConvert.SerializeObject(invite));
        }

        [TestMethod]
        public void ThrowsExceptionForNullableInvite()
        {
            var actual = Assert.ThrowsException<AggregateException>(
                () => userService.CreateInviteAsync("", null).Result);

            Assert.AreEqual(
                    string.Format(CultureInfo.CurrentCulture, ErrorMessages.ValueCannotBeNull, "invite"),
                    actual.InnerException?.Message);
        }
    }
}
