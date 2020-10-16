using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net.Model;
using SignNow.Net.Model.Requests;
using SignNow.Net.Service;
using SignNow.Net.Test;
using SignNow.Net.Test.Constants;

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

            StringAssert.Contains(exception.Message, "unverified_email");
            Assert.AreEqual("unverified_email", exception.InnerExceptions[0].Message);
        }
    }
}
