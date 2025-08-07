using System;
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
        /// An example of verifying a user's email address using the verification token from the verification email.
        /// To send the verification email to the user's email address, use the SendVerificationEmailAsync method first.
        /// </summary>
        [TestMethod]
        public async Task VerifyUserEmailAsync()
        {
            var clientId = credentials.ClientId;
            var clientSecret = credentials.ClientSecret;
            
            // Create Basic auth token for email verification
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes($"{clientId}:{clientSecret}");
            var appToken = Convert.ToBase64String(plainTextBytes);
            var basicToken = new Token { AppToken = appToken, TokenType = TokenType.Basic };
            
            var userService = new UserService(ApiBaseUrl, basicToken);

            // The verification token would normally come from the verification email
            var verificationToken = "your_verification_token_from_email";
            var userEmail = "user@example.com";

            try
            {
                var response = await userService.VerifyEmailAsync(userEmail, verificationToken)
                    .ConfigureAwait(false);

                Assert.IsNotNull(response);
                Assert.AreEqual(userEmail, response.Email);
                
                // If we reach here, the email has been successfully verified
                System.Console.WriteLine($"Email {response.Email} has been successfully verified.");
            }
            catch (SignNow.Net.Exceptions.SignNowException ex)
            {
                // Handle specific error cases as per API specification:
                // - Code 65629: verification token does not match email address or is invalid
                // - Code 65629: verification token is expired
                System.Console.WriteLine($"Email verification failed: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// An example showing the complete email verification flow.
        /// This demonstrates sending a verification email and then verifying it.
        /// </summary>
        [TestMethod]
        public async Task CompleteEmailVerificationFlowAsync()
        {
            var clientId = credentials.ClientId;
            var clientSecret = credentials.ClientSecret;
            
            // Create Basic auth token for email verification
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes($"{clientId}:{clientSecret}");
            var appToken = Convert.ToBase64String(plainTextBytes);
            var basicToken = new Token { AppToken = appToken, TokenType = TokenType.Basic };
            
            var userService = new UserService(ApiBaseUrl, basicToken);

            var userEmail = "user@example.com";

            // Step 1: Send verification email
            await userService.SendVerificationEmailAsync(userEmail)
                .ConfigureAwait(false);

            System.Console.WriteLine($"Verification email sent to {userEmail}");
            System.Console.WriteLine("Please check your email and extract the verification token from the verification link.");

            // Step 2: Verify email using token from the email
            // Note: In a real application, you would get this token from the user
            // after they click the verification link in their email
            var verificationToken = "token_from_verification_email";

            try
            {
                var response = await userService.VerifyEmailAsync(userEmail, verificationToken)
                    .ConfigureAwait(false);

                System.Console.WriteLine($"Email {response.Email} has been successfully verified!");
            }
            catch (SignNow.Net.Exceptions.SignNowException ex)
            {
                System.Console.WriteLine($"Email verification failed: {ex.Message}");
                
                // In a real application, you might want to:
                // - Ask the user to check their email again
                // - Resend the verification email
                // - Display appropriate error messages
            }
        }
    }
}
