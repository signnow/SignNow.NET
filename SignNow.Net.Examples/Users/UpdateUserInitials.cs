using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net.Extensions;

namespace SignNow.Net.Examples.Users
{
    /// <summary>
    /// Examples demonstrating how to update user initials signature.
    /// </summary>
    [TestClass]
    public class UpdateUserInitials : ExamplesBase
    {
        /// <summary>
        /// Updates user initials by reading from a PNG file.
        /// This demonstrates how to upload a user's initial signature from a file.
        /// </summary>
        [TestMethod]
        public async Task UpdateUserInitialsAsync()
        {
            var imagePath = Path.Combine(BaseTestExamplesPath, "test_initial.png");
            
            System.Console.WriteLine($"Reading image file: {imagePath}");
            
            // Read file directly as stream - no base64 conversion needed
            using var fileStream = File.OpenRead(imagePath);
            
            var response = await testContext.Users.UpdateUserInitialsAsync(fileStream)
                .ConfigureAwait(false);

            System.Console.WriteLine("User initials updated successfully!");
            System.Console.WriteLine($"Initial ID: {response.Id}");
            System.Console.WriteLine($"Dimensions: {response.Width}x{response.Height}");
            System.Console.WriteLine($"Created: {response.Created}");

            // Specific assertions according to code review
            Assert.IsTrue(response.Id.IsValidId(), "ID should match pattern ^[a-zA-Z0-9_]{40,40}$");
            Assert.AreEqual(80, response.Width, "Width should be 80 pixels");
            Assert.AreEqual(40, response.Height, "Height should be 40 pixels");
            Assert.IsNotNull(response.Created, "Created timestamp should not be null");
        }

        /// <summary>
        /// Updates user initials by reading from a PNG file using MemoryStream.
        /// This demonstrates how to upload a user's initial signature from binary data.
        /// </summary>
        [TestMethod]
        public async Task UpdateUserInitialsFromMemoryStreamAsync()
        {
            var imagePath = Path.Combine(BaseTestExamplesPath, "test_initial.png");
            
            System.Console.WriteLine($"Reading image file: {imagePath}");
            
            // Read file into memory stream for different use case
            var imageBytes = await File.ReadAllBytesAsync(imagePath);
            using var memoryStream = new MemoryStream(imageBytes);
            
            var response = await testContext.Users.UpdateUserInitialsAsync(memoryStream)
                .ConfigureAwait(false);

            System.Console.WriteLine("User initials updated successfully from memory stream!");
            System.Console.WriteLine($"Initial ID: {response.Id}");
            System.Console.WriteLine($"Dimensions: {response.Width}x{response.Height}");
            System.Console.WriteLine($"Created: {response.Created}");

            // Specific assertions according to code review
            Assert.IsTrue(response.Id.IsValidId(), "ID should match pattern ^[a-zA-Z0-9_]{40,40}$");
            Assert.AreEqual(80, response.Width, "Width should be 80 pixels");
            Assert.AreEqual(40, response.Height, "Height should be 40 pixels");
            Assert.IsNotNull(response.Created, "Created timestamp should not be null");
        }
    }
}
