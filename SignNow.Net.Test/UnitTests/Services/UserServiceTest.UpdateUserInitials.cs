using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net.Exceptions;
using SignNow.Net.Model;
using SignNow.Net.Model.Responses;
using SignNow.Net.Service;

namespace UnitTests.Services
{
    public partial class UserServiceTest
    {
        [TestMethod]
        public void UpdateUserInitialsWithValidImageDataShouldReturnResponse()
        {
            // Real API response structure based on sn-api-mvp.json specification
            var mockResponse = @"
            {
                ""id"": ""1234567890abcdef1234567890abcdef12345678"",
                ""width"": ""80"",
                ""height"": ""40"",
                ""created"": ""1701424800""
            }";

            var userService = new UserService(ApiBaseUrl, new Token(), SignNowClientMock(mockResponse));
            var validImageBytes = Convert.FromBase64String("iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAYAAAAfFcSJAAAADUlEQVR42mNk+M9QDwADhgGAWjR9awAAAABJRU5ErkJggg==");
            
            using var imageStream = new MemoryStream(validImageBytes);
            var response = userService.UpdateUserInitialsAsync(imageStream).Result;

            Assert.IsNotNull(response);
            Assert.AreEqual("1234567890abcdef1234567890abcdef12345678", response.Id);
            Assert.AreEqual(80, response.Width);
            Assert.AreEqual(40, response.Height);
            Assert.AreEqual(new DateTime(2023, 12, 1, 10, 0, 0, DateTimeKind.Utc), response.Created);
        }

        [TestMethod]
        public void UpdateUserInitialsShouldThrowExceptionForNullImageData()
        {
            var userService = new UserService(ApiBaseUrl, new Token());

            var exception = Assert.ThrowsException<AggregateException>(
                () => userService.UpdateUserInitialsAsync(null).Result);

            Assert.IsInstanceOfType(exception.InnerException, typeof(ArgumentNullException));
            Assert.AreEqual("imageData", ((ArgumentNullException)exception.InnerException).ParamName);
        }

        [TestMethod]
        public async Task UpdateUserInitialsShouldThrowExceptionForEmptyImageData()
        {
            // Real API error response for empty data - based on comment in code review
            var errorResponse = @"
            {
                ""errors"": [
                    {
                        ""code"": 65536,
                        ""message"": ""data must not be empty""
                    }
                ]
            }";

            var userService = new UserService(ApiBaseUrl, new Token(), SignNowClientMock(errorResponse, HttpStatusCode.BadRequest));
            using var emptyStream = new MemoryStream();
            
            var exception = await Assert.ThrowsExceptionAsync<SignNowException>(
                async () => await userService.UpdateUserInitialsAsync(emptyStream));

            Assert.IsNotNull(exception);
            Assert.IsTrue(exception.Message.Contains("data must not be empty"), 
                $"Expected 'data must not be empty' in error message. Actual: {exception.Message}");
        }

        [TestMethod]
        public async Task UpdateUserInitialsWithInvalidImageDataShouldThrowSignNowException()
        {
            // Real API error response for invalid payload based on user.json code 65536
            var errorResponse = @"
            {
                ""errors"": [
                    {
                        ""code"": 65536,
                        ""message"": ""Invalid payload""
                    }
                ]
            }";

            var userService = new UserService(ApiBaseUrl, new Token(), SignNowClientMock(errorResponse, HttpStatusCode.BadRequest));
            var invalidImageBytes = System.Text.Encoding.UTF8.GetBytes("invalid image data");
            
            using var imageStream = new MemoryStream(invalidImageBytes);
            var exception = await Assert.ThrowsExceptionAsync<SignNowException>(
                async () => await userService.UpdateUserInitialsAsync(imageStream));

            Assert.IsNotNull(exception);
            Assert.IsTrue(exception.Message.Contains("Invalid payload") || exception.Message.Contains("invalid"), 
                $"Expected error message to contain validation error. Actual: {exception.Message}");
        }

        [TestMethod]
        public async Task UpdateUserInitialsWithUnsupportedImageTypeShouldThrowSignNowException()
        {
            // Real API error response for unsupported format
            var errorResponse = @"
            {
                ""errors"": [
                    {
                        ""code"": 65536,
                        ""message"": ""Invalid payload""
                    }
                ]
            }";

            var userService = new UserService(ApiBaseUrl, new Token(), SignNowClientMock(errorResponse, HttpStatusCode.BadRequest));
            var unsupportedImageBytes = System.Text.Encoding.UTF8.GetBytes("GIF89a..."); // Unsupported GIF data
            
            using var imageStream = new MemoryStream(unsupportedImageBytes);
            var exception = await Assert.ThrowsExceptionAsync<SignNowException>(
                async () => await userService.UpdateUserInitialsAsync(imageStream));

            Assert.IsNotNull(exception);
            Assert.IsTrue(exception.Message.Contains("Invalid payload") || exception.Message.Contains("invalid"), 
                $"Expected error message to contain validation error. Actual: {exception.Message}");
        }

        [TestMethod]
        public async Task UpdateUserInitialsWithImageTooLargeShouldThrowSignNowException()
        {
            // Real API error response for payload too large
            var errorResponse = @"
            {
                ""errors"": [
                    {
                        ""code"": 65536,
                        ""message"": ""Invalid payload""
                    }
                ]
            }";

            var userService = new UserService(ApiBaseUrl, new Token(), SignNowClientMock(errorResponse, HttpStatusCode.BadRequest));
            var largeImageBytes = new byte[10 * 1024 * 1024]; // 10MB of data
            
            using var imageStream = new MemoryStream(largeImageBytes);
            var exception = await Assert.ThrowsExceptionAsync<SignNowException>(
                async () => await userService.UpdateUserInitialsAsync(imageStream));

            Assert.IsNotNull(exception);
            Assert.IsTrue(exception.Message.Contains("Invalid payload") || exception.Message.Contains("invalid"), 
                $"Expected error message to contain validation error. Actual: {exception.Message}");
        }
    }
}