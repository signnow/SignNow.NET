using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net.Model;
using SignNow.Net.Model.Requests;
using SignNow.Net.Service;
using SignNow.Net.Test.Constants;
using SignNow.Net.Test.FakeModels;

namespace UnitTests
{
    [TestClass]
    public class UserServiceTest : SignNowTestBase
    {
        [TestMethod]
        public void ThrowsExceptionOnInviteIsNull()
        {
            var service = new UserService(new Token());
            var response = service.CancelInviteAsync(null as FreeformInvite).Exception;

            Assert.IsNotNull(response);
            StringAssert.Contains(response.InnerException?.Message, ErrorMessages.ValueCannotBeNull);
            StringAssert.Contains(response.InnerException?.Message, "invite");
        }

        [TestMethod]
        public void CreateUser()
        {
            var mockResponse = @"
            {
                ""Id"": ""aad13814e8f369089ee2f12b533545f9534e6cf8"",
                ""Verified"": false,
                ""Email"": ""user@signnow.com""
            }";

            var userService = new UserService(ApiBaseUrl, new Token(), SignNowClientMock(mockResponse));

            var userRequest = new CreateUserOptions
            {
                Email = "user@signnow.com",
                FirstName = "John",
                LastName = "Test",
                Password = "my***secret***password"
            };

            var response = userService.CreateUserAsync(userRequest).Result;

            Assert.AreEqual(userRequest.Email, response.Email);
            Assert.IsFalse(response.Verified);
        }


        [TestMethod]
        public void UpdateUserDetails()
        {
            var mockResponse = @"
            {
                ""id"": ""aad13814e8f369089ee2f12b533545f9534e6cf8"",
                ""first_name"": ""John"",
                ""last_name"": ""Snow""
            }";

            var userService = new UserService(ApiBaseUrl, new Token(), SignNowClientMock(mockResponse));
            var userUpdateOptions = new UpdateUserOptions
            {
                FirstName = "John",
                LastName = "Snow",
                OldPassword = "verystrongpassword",
                Password = "superverystrongpassword",
                LogOutAll = false
            };

            var updated = userService.UpdateUserAsync(userUpdateOptions).Result;

            Assert.AreEqual("aad13814e8f369089ee2f12b533545f9534e6cf8", updated.Id);
            Assert.AreEqual("John", updated.FirstName);
            Assert.AreEqual("Snow", updated.LastName);
        }

        [TestMethod]
        public void SendVerificationEmail()
        {
            var mockResponse = @"
            {
                ""email"": ""user@signnow.com""
            }";

            var userService = new UserService(ApiBaseUrl, new Token(), SignNowClientMock(mockResponse));

            var sendTask = userService.SendVerificationEmailAsync("user@signnow.com");

            Task.WaitAll(sendTask);
            Assert.IsTrue(sendTask.IsCompleted);
            Assert.AreEqual(TaskStatus.RanToCompletion, sendTask.Status);
        }

        [TestMethod]
        public void SendPasswordResetLinkToUser()
        {
            var mockResponse = @"
            {
                ""status"": ""success""
            }";

            var userService = new UserService(ApiBaseUrl, new Token(), SignNowClientMock(mockResponse));

            var sendTask = userService.SendPasswordResetLinkAsync("user@signnow.com");

            Task.WaitAll(sendTask);
            Assert.IsTrue(sendTask.IsCompleted);
            Assert.AreEqual(TaskStatus.RanToCompletion, sendTask.Status);
        }

        [TestMethod]
        public void HandleErrorsWhenCreateUser()
        {
            var mockResponse = "{\"errors\": [{\"code\": 66396, \"message\": \"unverified_email\"}]}";
            var userService = new UserService(ApiBaseUrl, new Token(), SignNowClientMock(mockResponse, HttpStatusCode.BadRequest));

            var exception = Assert.ThrowsException<AggregateException>(
                () => userService.CreateUserAsync(new CreateUserOptions()).Result);

            var expectedMessage = "unverified_email";

#if NETFRAMEWORK
            expectedMessage = "One or more errors occurred.";
#endif
            StringAssert.Contains(exception.Message, expectedMessage);
            Assert.AreEqual("unverified_email", exception.InnerExceptions[0].Message);
        }

        [TestMethod]
        public void CreatesEmbeddedSigningInvite()
        {
            var mockResponse = @"
                {
                  ""data"": [
                    {
                      ""id"": ""139f1a566dc64946ab5fe73883811efd8136deb7"",
                      ""email"": ""test.mail@gmail.com"",
                      ""role_id"": ""c6fa0bf1f4e2045c7fbcf6335a26bef94b7199b7"",
                      ""order"": 1,
                      ""status"": ""pending""
                    }
                  ]
                }";

            var document = new SignNowDocumentFaker()
                .FinishWith((f, o) =>
                {
                    o.Roles = new RoleFaker().Generate(1);
                })
                .Generate();
            var embedded = new EmbeddedSigningInvite(document);
            embedded.AddEmbeddedSigningInvite(
                new EmbeddedInviteFaker()
                    .FinishWith((f, o) =>
                    {
                        o.RoleId = document.Roles.First().Id;
                    })
                    .Generate());

            var userService = new UserService(ApiBaseUrl, new Token(), SignNowClientMock(mockResponse));
            var embeddedInvite = userService.CreateInviteAsync(document.Id, embedded).Result;

            Assert.IsNotNull(embeddedInvite);
            Assert.AreEqual("139f1a566dc64946ab5fe73883811efd8136deb7", embeddedInvite.InviteData[0].Id);
            Assert.AreEqual("Pending", embeddedInvite.InviteData[0].Status.ToString());
        }

        [TestMethod]
        public void GenerateEmbeddedSigningLink()
        {
            var mockResponse = @"
            {
              ""data"": {
                ""link"": ""https://app-eval.signnow.com/webapp/document/9dbdffc9c5af49809d4dfdf613e9835b50a9582f?access_token=f108a715acd9272b7b25a3ecd2bc06962148de40b2ecdd08c05fbf41994b98b6&route=fieldinvite""
              }
            }";

            var document = new SignNowDocumentFaker()
                .FinishWith((f, o) =>
                {
                    o.Roles = new RoleFaker().Generate(1);
                })
                .Generate();

            var options = new CreateEmbedLinkOptions
            {
                FieldInvite = new FieldInviteFaker().Generate(),
                AuthMethod = EmbeddedAuthType.None,
                LinkExpiration = 30
            };

            var userService = new UserService(ApiBaseUrl, new Token(), SignNowClientMock(mockResponse));
            var embeddedLink = userService.GenerateEmbeddedInviteLinkAsync(document.Id, options).Result;
            var link =
                "https://app-eval.signnow.com/webapp/document/9dbdffc9c5af49809d4dfdf613e9835b50a9582f?access_token=f108a715acd9272b7b25a3ecd2bc06962148de40b2ecdd08c05fbf41994b98b6&route=fieldinvite";

            Assert.AreEqual(link, embeddedLink.Link.AbsoluteUri);
        }
    }
}
