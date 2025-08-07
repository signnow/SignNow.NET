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
        public void UpdateUserInitialsWithValidImageDataShouldReturnResponse()
        {
            var mockResponse = @"
            {
                ""id"": ""initial123"",
                ""width"": ""200"",
                ""height"": ""100"",
                ""created"": ""2023-12-01T10:00:00Z""
            }";

            var userService = new UserService(ApiBaseUrl, new Token(), SignNowClientMock(mockResponse));
            var validImageData = "iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAYAAAAfFcSJAAAADUlEQVR42mNk+M9QDwADhgGAWjR9awAAAABJRU5ErkJggg==";

            var response = userService.UpdateUserInitialsAsync(validImageData).Result;

            Assert.IsNotNull(response);
            Assert.AreEqual("initial123", response.Id);
            Assert.AreEqual("200", response.Width);
            Assert.AreEqual("100", response.Height);
            Assert.AreEqual("2023-12-01T10:00:00Z", response.Created);
        }

        [TestMethod]
        public void UpdateUserInitialsShouldThrowExceptionForNullImageData()
        {
            var userService = new UserService(ApiBaseUrl, new Token());

            var exception = Assert.ThrowsException<AggregateException>(
                () => userService.UpdateUserInitialsAsync(null).Result);

            Assert.IsNotNull(exception.InnerException);
            StringAssert.Contains(exception.InnerException.Message, "Cannot be null, empty or whitespace");
            StringAssert.Contains(exception.InnerException.Message, "imageData");
        }

        [TestMethod]
        public void UpdateUserInitialsShouldThrowExceptionForEmptyImageData()
        {
            var userService = new UserService(ApiBaseUrl, new Token());

            var exception = Assert.ThrowsException<AggregateException>(
                () => userService.UpdateUserInitialsAsync("").Result);

            Assert.IsNotNull(exception.InnerException);
            StringAssert.Contains(exception.InnerException.Message, "Cannot be null, empty or whitespace");
            StringAssert.Contains(exception.InnerException.Message, "imageData");
        }

        [TestMethod]
        public async Task UpdateUserInitialsWithInvalidImageDataShouldThrowSignNowException()
        {
            var mockErrorResponse = @"
            {
                ""error"": ""Unable to convert file to png"",
                ""error_description"": ""Unable to convert file to png""
            }";

            var userService = new UserService(ApiBaseUrl, new Token(), 
                SignNowClientMock(mockErrorResponse, System.Net.HttpStatusCode.BadRequest));

            var exception = await Assert.ThrowsExceptionAsync<SignNowException>(
                async () => await userService.UpdateUserInitialsAsync("invalid_image_data"));

            Assert.IsNotNull(exception);
            StringAssert.Contains(exception.Message, "Unable to convert file to png");
        }

        [TestMethod]
        public async Task UpdateUserInitialsWithUnsupportedImageTypeShouldThrowSignNowException()
        {
            var mockErrorResponse = @"
            {
                ""error"": ""Unsupported Image Type"",
                ""error_description"": ""Unsupported Image Type""
            }";

            var userService = new UserService(ApiBaseUrl, new Token(), 
                SignNowClientMock(mockErrorResponse, System.Net.HttpStatusCode.BadRequest));

            var exception = await Assert.ThrowsExceptionAsync<SignNowException>(
                async () => await userService.UpdateUserInitialsAsync("unsupported_format_data"));

            Assert.IsNotNull(exception);
            StringAssert.Contains(exception.Message, "Unsupported Image Type");
        }

        [TestMethod]
        public async Task UpdateUserInitialsWithImageTooLargeShouldThrowSignNowException()
        {
            var mockErrorResponse = @"
            {
                ""error"": ""Initial image too large."",
                ""error_description"": ""Initial image too large.""
            }";

            var userService = new UserService(ApiBaseUrl, new Token(), 
                SignNowClientMock(mockErrorResponse, System.Net.HttpStatusCode.BadRequest));

            var exception = await Assert.ThrowsExceptionAsync<SignNowException>(
                async () => await userService.UpdateUserInitialsAsync("very_large_image_data"));

            Assert.IsNotNull(exception);
            StringAssert.Contains(exception.Message, "Initial image too large");
        }
    }
}
