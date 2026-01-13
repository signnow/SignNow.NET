using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net.Exceptions;
using SignNow.Net.Model;
using SignNow.Net.Model.Responses;
using SignNow.Net.Service;
using SignNow.Net.Test.Constants;

namespace UnitTests.Services
{
    public partial class UserServiceTest
    {
        [TestMethod]
        public void VerifyEmailWithValidTokenShouldSucceed()
        {
            var mockResponse = @"
            {
                ""email"": ""user@signnow.com""
            }";

            var userService = new UserService(ApiBaseUrl, new Token(), SignNowClientMock(mockResponse));

            var response = userService.VerifyEmailAsync("user@signnow.com", "valid_verification_token").Result;

            Assert.IsNotNull(response);
            Assert.AreEqual("user@signnow.com", response.Email);
        }

        [TestMethod]
        public void VerifyEmailShouldThrowExceptionForInvalidEmail()
        {
            var userService = new UserService(ApiBaseUrl, new Token());

            var exception = Assert.ThrowsException<AggregateException>(
                () => userService.VerifyEmailAsync("invalid-email", "valid_token").Result);

            Assert.IsNotNull(exception.InnerException);
            StringAssert.Contains(exception.InnerException.Message, "Invalid format of email");
        }

        [TestMethod]
        public void VerifyEmailShouldThrowExceptionForNullToken()
        {
            var userService = new UserService(ApiBaseUrl, new Token());

            var exception = Assert.ThrowsException<AggregateException>(
                () => userService.VerifyEmailAsync("user@signnow.com", null).Result);

            Assert.IsNotNull(exception.InnerException);
            StringAssert.Contains(exception.InnerException.Message, "Cannot be null, empty or whitespace");
            StringAssert.Contains(exception.InnerException.Message, "verificationToken");
        }

        [TestMethod]
        public void VerifyEmailShouldThrowExceptionForEmptyToken()
        {
            var userService = new UserService(ApiBaseUrl, new Token());

            var exception = Assert.ThrowsException<AggregateException>(
                () => userService.VerifyEmailAsync("user@signnow.com", "").Result);

            Assert.IsNotNull(exception.InnerException);
            StringAssert.Contains(exception.InnerException.Message, "Cannot be null, empty or whitespace");
            StringAssert.Contains(exception.InnerException.Message, "verificationToken");
        }

        [TestMethod]
        public async Task VerifyEmailWithInvalidTokenShouldThrowSignNowException()
        {
            var mockErrorResponse = @"
            {
                ""errors"": [
                    {
                        ""code"": 65629,
                        ""message"": ""verification token does not match email address passed in or is invalid""
                    }
                ]
            }";

            var userService = new UserService(ApiBaseUrl, new Token(), 
                SignNowClientMock(mockErrorResponse, System.Net.HttpStatusCode.BadRequest));

            var exception = await Assert.ThrowsExceptionAsync<SignNowException>(
                async () => await userService.VerifyEmailAsync("user@signnow.com", "invalid_token"));

            Assert.IsNotNull(exception);
        }

        [TestMethod]
        public async Task VerifyEmailWithExpiredTokenShouldThrowSignNowException()
        {
            var mockErrorResponse = @"
            {
                ""errors"": [
                    {
                        ""code"": 65629,
                        ""message"": ""Verification token is expired""
                    }
                ]
            }";

            var userService = new UserService(ApiBaseUrl, new Token(), 
                SignNowClientMock(mockErrorResponse, System.Net.HttpStatusCode.BadRequest));

            var exception = await Assert.ThrowsExceptionAsync<SignNowException>(
                async () => await userService.VerifyEmailAsync("user@signnow.com", "expired_token"));

            Assert.IsNotNull(exception);
        }
    }
}
