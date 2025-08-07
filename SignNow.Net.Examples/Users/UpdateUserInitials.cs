using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net.Model;
using SignNow.Net.Service;
using SignNow.Net.Test.Context;

namespace SignNow.Net.Examples
{
    public partial class UserExamples : ExamplesBase
    {
        /// <summary>
        /// An example of updating user initials with base64 encoded image data.
        /// This demonstrates how to upload a user's initial signature using image data.
        /// </summary>
        [TestMethod]
        public async Task UpdateUserInitialsAsync()
        {
            // Real base64 data from test_initial.png that works in Postman (80x40 PNG)
            var imageData = "iVBORw0KGgoAAAANSUhEUgAAAFAAAAAoBAMAAACbTmAyAAAAG1BMVEUzMzP///9/f3+ysrKZmZnl5eXMzMxMTExmZmbo11sgAAAACXBIWXMAAA7EAAAOxAGVKw4bAAAAk0lEQVQ4jWNgGAVDBDAZMLilGXsUEFTIEsBg6uJiKkxQJVChGZBqFiJSIYOiApEKWcSJVMgQQqxCV2IVNhLwN1xhYQORChmprpBoq4n2DKHgYU4gMsDZoAqZCEUhowNEYaMCAYWOBWCFLCL4FBkVMBQDFZiVVyiHNuBT6CoYKAq00VBQNBmvOgYG9g7CeWUU0AcAAA7tGrTqxEvjAAAAAElFTkSuQmCC";

            try
            {
                System.Console.WriteLine("Updating user initials...");

                // Use testContext like other working examples - this is the key difference!
                var response = await testContext.Users.UpdateUserInitialsAsync(imageData)
                    .ConfigureAwait(false);

                System.Console.WriteLine($"User initials updated successfully!");
                System.Console.WriteLine($"Initial ID: {response.Id}");
                System.Console.WriteLine($"Dimensions: {response.Width}x{response.Height}");
                System.Console.WriteLine($"Created: {response.Created}");

                Assert.IsNotNull(response);
                Assert.IsNotNull(response.Id);
                Assert.IsNotNull(response.Width);
                Assert.IsNotNull(response.Height);
                Assert.IsNotNull(response.Created);
            }
            catch (SignNow.Net.Exceptions.SignNowException ex)
            {
                // Handle specific error cases as per API specification:
                // - Unable to convert file to png
                // - Unsupported Image Type
                // - Initial image too large
                System.Console.WriteLine($"Request failed: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// An example of updating user initials from an image file.
        /// This demonstrates how to upload a user's initial signature from a file.
        /// </summary>
        [TestMethod]
        public async Task UpdateUserInitialsFromFileAsync()
        {

            // Path to test image file (you can change this to any valid image file)
            var imagePath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "Assets", "Images", "test_initial.png"));

            try
            {
                System.Console.WriteLine($"Reading image file: {imagePath}");

                // Check if file exists, if not use base64 data as fallback
                string imageData;
                if (File.Exists(imagePath))
                {
                    var imageBytes = await File.ReadAllBytesAsync(imagePath);
                    imageData = Convert.ToBase64String(imageBytes);
                }
                else
                {
                    System.Console.WriteLine("Test file not found, using sample base64 data");
                    // Fallback to 1x1 pixel PNG
                    imageData = "iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAYAAAAfFcSJAAAADUlEQVR42mNk+M9QDwADhgGAWjR9awAAAABJRU5ErkJggg==";
                }

                System.Console.WriteLine("Updating user initials from file...");

                // Use testContext like other working examples - this is the key difference!
                var response = await testContext.Users.UpdateUserInitialsAsync(imageData)
                    .ConfigureAwait(false);

                System.Console.WriteLine($"User initials updated successfully from file!");
                System.Console.WriteLine($"Initial ID: {response.Id}");
                System.Console.WriteLine($"Dimensions: {response.Width}x{response.Height}");
                System.Console.WriteLine($"Created: {response.Created}");

                Assert.IsNotNull(response);
                Assert.IsNotNull(response.Id);
            }
            catch (SignNow.Net.Exceptions.SignNowException ex)
            {
                System.Console.WriteLine($"Request failed: {ex.Message}");
                throw;
            }
            catch (FileNotFoundException)
            {
                System.Console.WriteLine($"Image file not found: {imagePath}");
                throw;
            }
        }
    }
}
