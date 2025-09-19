using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net.Exceptions;

namespace AcceptanceTests
{
    public partial class UserServiceTest
    {
        [DataTestMethod]
        [DynamicData(nameof(GetInvalidImageDataTestCases), DynamicDataSourceType.Method)]
        public async Task CannotUpdateUserInitialsWithInvalidData(string testName, byte[] imageData, string expectedErrorMessage)
        {
            using var imageStream = new MemoryStream(imageData);
            
            var exception = await Assert.ThrowsExceptionAsync<SignNowException>(
                async () => await SignNowTestContext.Users.UpdateUserInitialsAsync(imageStream));

            Assert.IsNotNull(exception, "Exception should not be null");
            
            // Check for specific API error message
            Assert.IsTrue(
                exception.Message.IndexOf(expectedErrorMessage, StringComparison.OrdinalIgnoreCase) >= 0,
                $"Test case '{testName}': Expected error message to contain '{expectedErrorMessage}'. Actual: {exception.Message}"
            );
        }

        [TestMethod]
        public async Task CannotUpdateUserInitialsWithNullImageData()
        {
            var exception = await Assert.ThrowsExceptionAsync<ArgumentNullException>(
                async () => await SignNowTestContext.Users.UpdateUserInitialsAsync(null));

            Assert.IsNotNull(exception, "Exception should not be null");
            Assert.AreEqual("imageData", exception.ParamName, "Parameter name should be 'imageData'");
        }

        private static IEnumerable<object[]> GetInvalidImageDataTestCases()
        {
            // Test case: Empty data should return "data must not be empty" error
            yield return new object[] 
            { 
                "Empty Image Data", 
                new byte[0], 
                "data must not be empty" 
            };

            // Test case: Invalid image format - API returns specific error message
            yield return new object[] 
            { 
                "Invalid Image Format", 
                Encoding.UTF8.GetBytes("invalid image data"), 
                "Unable to convert file to png" 
            };

            // Test case: Unsupported image type (e.g., GIF) - API returns specific error message
            yield return new object[] 
            { 
                "Unsupported Image Type", 
                Encoding.UTF8.GetBytes("GIF89a..."), 
                "Unable to convert file to png" 
            };
        }
    }
}
