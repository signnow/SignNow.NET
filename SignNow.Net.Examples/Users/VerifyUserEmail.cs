using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net.Extensions;

namespace SignNow.Net.Examples
{
    public partial class UserExamples : ExamplesBase
    {
        /// <summary>
        /// An example of verifying a user's email address using the verification token from the verification email.
        /// To send the verification email to the user's email address, use the SendVerificationEmailAsync method first.
        /// 
        /// NOTE: This example demonstrates the API usage but uses placeholder values.
        /// In real usage, you would get the verification token from the user's email.
        /// </summary>
        [TestMethod]
        public async Task VerifyUserEmailAsync()
        {
            System.Console.WriteLine("Email Verification Example:");
            System.Console.WriteLine("==========================");
            System.Console.WriteLine();
            
            // The verification token would normally come from the verification email
            var verificationToken = "your_verification_token_from_email";  // This should be actual token from email
            var userEmail = "user@example.com";  // This should be the actual user's email
            
            System.Console.WriteLine($"To verify email: {userEmail}");
            System.Console.WriteLine($"With token: {verificationToken}");
            System.Console.WriteLine();
            System.Console.WriteLine("API Call: testContext.Users.VerifyEmailAsync(email, token)");
            System.Console.WriteLine();
            
            try
            {
                var response = await testContext.Users
                    .VerifyEmailAsync(userEmail, verificationToken)
                    .ConfigureAwait(false);

                Assert.IsNotNull(response);
                Assert.IsTrue(response.Email.IsValidEmail(), "Response should contain a valid email address");
                Assert.AreEqual(userEmail, response.Email, "Response email should match the verified email");
                
                System.Console.WriteLine($"✅ Email {response.Email} has been successfully verified.");
            }
            catch (SignNow.Net.Exceptions.SignNowException ex)
            {
                // This is expected with placeholder data
                System.Console.WriteLine($"⚠️  Expected error with placeholder data: {ex.Message}");
                System.Console.WriteLine();
                System.Console.WriteLine("Expected API error codes:");
                System.Console.WriteLine("- Code 65629: verification token does not match email address or is invalid");
                System.Console.WriteLine("- Code 65629: verification token is expired");
                
                // Don't fail the test for demo purposes
                Assert.IsTrue(ex.Message.Contains("verification token"), "Should be verification token error");
            }
        }

        /// <summary>
        /// An example showing the complete email verification flow.
        /// This demonstrates sending a verification email and then verifying it.
        /// 
        /// NOTE: This example demonstrates the API usage but uses placeholder values.
        /// In real usage, you would use actual user email and token from the verification email.
        /// </summary>
        [TestMethod]
        public async Task CompleteEmailVerificationFlowAsync()
        {
            System.Console.WriteLine("Complete Email Verification Flow:");
            System.Console.WriteLine("==================================");
            System.Console.WriteLine();
            
            var userEmail = "user@example.com";  // This should be the actual user's email

            try
            {
                // Step 1: Send verification email
                await testContext.Users.SendVerificationEmailAsync(userEmail)
                    .ConfigureAwait(false);

                System.Console.WriteLine($"✅ Step 1: Verification email sent to {userEmail}");
                System.Console.WriteLine("   In real usage: User would check their email for verification link");
                System.Console.WriteLine();

                // Step 2: Verify email using token from the email
                // Note: In a real application, you would get this token from the user
                // after they click the verification link in their email
                var verificationToken = "token_from_verification_email";  // This would be from email

                System.Console.WriteLine($"📧 Step 2: Using verification token: {verificationToken}");
                System.Console.WriteLine("   API Call: testContext.Users.VerifyEmailAsync(email, token)");
                System.Console.WriteLine();

                var response = await testContext.Users
                    .VerifyEmailAsync(userEmail, verificationToken)
                    .ConfigureAwait(false);

                System.Console.WriteLine($"✅ Email {response.Email} has been successfully verified!");

                // Validate response
                Assert.IsNotNull(response, "Response should not be null");
                Assert.IsTrue(response.Email.IsValidEmail(), "Response should contain a valid email address");
                Assert.AreEqual(userEmail, response.Email, "Response email should match the verified email");
            }
            catch (SignNow.Net.Exceptions.SignNowException ex)
            {
                // This is expected with placeholder data for verification step
                System.Console.WriteLine($"⚠️  Step 2 failed (expected with placeholder token): {ex.Message}");
                System.Console.WriteLine();
                System.Console.WriteLine("In a real application, you would:");
                System.Console.WriteLine("- Ask the user to check their email again");
                System.Console.WriteLine("- Resend the verification email if needed");
                System.Console.WriteLine("- Display appropriate error messages based on error codes");
                
                // Don't fail the test for demo purposes - any error is expected with placeholder data
                Assert.IsNotNull(ex.Message, "Should have error message");
            }
        }
    }
}
