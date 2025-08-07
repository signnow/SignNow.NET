using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net.Exceptions;
using SignNow.Net.Model;
using SignNow.Net.Service;
using SignNow.Net.Test.Context;

namespace AcceptanceTests
{
    public partial class UserServiceTest
    {
        [TestMethod]
        public async Task CannotVerifyEmailWithIncorrectToken()
        {
            var credentials = new CredentialLoader(ApiBaseUrl).GetCredentials();
            var userService = new UserService(ApiBaseUrl, null);
            
            // Create Basic auth token like in OAuth2Service
            var plainTextBytes = Encoding.UTF8.GetBytes($"{credentials.ClientId}:{credentials.ClientSecret}");
            var appToken = Convert.ToBase64String(plainTextBytes);
            userService.Token = new Token { AppToken = appToken, TokenType = TokenType.Basic };
            
            var exception = await Assert.ThrowsExceptionAsync<SignNowException>(
                async () => await userService.VerifyEmailAsync(
                    "test@signnow.com", 
                    "incorrect_verification_token"));

            // Should throw exception for incorrect token as per API specification
            Assert.IsNotNull(exception);
        }

        [TestMethod]
        public void VerifyEmailWithNullTokenShouldThrowArgumentException()
        {
            var credentials = new CredentialLoader(ApiBaseUrl).GetCredentials();
            var userService = new UserService(ApiBaseUrl, null);
            
            // Create Basic auth token like in OAuth2Service
            var plainTextBytes = Encoding.UTF8.GetBytes($"{credentials.ClientId}:{credentials.ClientSecret}");
            var appToken = Convert.ToBase64String(plainTextBytes);
            userService.Token = new Token { AppToken = appToken, TokenType = TokenType.Basic };

            var exception = Assert.ThrowsException<AggregateException>(
                () => userService.VerifyEmailAsync("test@signnow.com", null).Result);

            Assert.IsNotNull(exception.InnerException);
            Assert.IsInstanceOfType(exception.InnerException, typeof(ArgumentException));
        }

        [TestMethod]
        public void VerifyEmailWithInvalidEmailShouldThrowArgumentException()
        {
            var credentials = new CredentialLoader(ApiBaseUrl).GetCredentials();
            var userService = new UserService(ApiBaseUrl, null);
            
            // Create Basic auth token like in OAuth2Service
            var plainTextBytes = Encoding.UTF8.GetBytes($"{credentials.ClientId}:{credentials.ClientSecret}");
            var appToken = Convert.ToBase64String(plainTextBytes);
            userService.Token = new Token { AppToken = appToken, TokenType = TokenType.Basic };

            var exception = Assert.ThrowsException<AggregateException>(
                () => userService.VerifyEmailAsync("invalid-email", "valid_token").Result);

            Assert.IsNotNull(exception.InnerException);
            Assert.IsInstanceOfType(exception.InnerException, typeof(ArgumentException));
        }
    }
}
