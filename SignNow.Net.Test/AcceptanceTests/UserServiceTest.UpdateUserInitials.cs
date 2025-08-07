using System;
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
        public async Task CannotUpdateUserInitialsWithIncorrectImageData()
        {
            var credentials = new CredentialLoader(ApiBaseUrl).GetCredentials();
            var userService = new UserService(ApiBaseUrl, null);

            // Get Bearer token for user initial operations
            var oAuthService = new OAuth2Service(ApiBaseUrl, credentials.ClientId, credentials.ClientSecret);
            var token = await oAuthService.GetTokenAsync(credentials.Login, credentials.Password, SignNow.Net.Model.Scope.All);
            userService.Token = token;

            var exception = await Assert.ThrowsExceptionAsync<SignNowException>(
                async () => await userService.UpdateUserInitialsAsync("invalid_image_data"));

            // Should throw exception for incorrect image data as per API specification
            Assert.IsNotNull(exception);
        }

        [TestMethod]
        public async Task CannotUpdateUserInitialsWithEmptyImageData()
        {
            var credentials = new CredentialLoader(ApiBaseUrl).GetCredentials();
            var userService = new UserService(ApiBaseUrl, null);

            // Get Bearer token for user initial operations
            var oAuthService = new OAuth2Service(ApiBaseUrl, credentials.ClientId, credentials.ClientSecret);
            var token = await oAuthService.GetTokenAsync(credentials.Login, credentials.Password, SignNow.Net.Model.Scope.All);
            userService.Token = token;

            var exception = await Assert.ThrowsExceptionAsync<ArgumentException>(
                async () => await userService.UpdateUserInitialsAsync(""));

            Assert.IsNotNull(exception);
            StringAssert.Contains(exception.Message, "imageData");
        }

        [TestMethod]
        public async Task CannotUpdateUserInitialsWithNullImageData()
        {
            var credentials = new CredentialLoader(ApiBaseUrl).GetCredentials();
            var userService = new UserService(ApiBaseUrl, null);

            // Get Bearer token for user initial operations
            var oAuthService = new OAuth2Service(ApiBaseUrl, credentials.ClientId, credentials.ClientSecret);
            var token = await oAuthService.GetTokenAsync(credentials.Login, credentials.Password, SignNow.Net.Model.Scope.All);
            userService.Token = token;

            var exception = await Assert.ThrowsExceptionAsync<ArgumentException>(
                async () => await userService.UpdateUserInitialsAsync(null));

            Assert.IsNotNull(exception);
            StringAssert.Contains(exception.Message, "imageData");
        }
    }
}
