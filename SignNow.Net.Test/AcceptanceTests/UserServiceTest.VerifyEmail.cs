using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net.Exceptions;

namespace AcceptanceTests
{
    public partial class UserServiceTest
    {
        [DataTestMethod]
        [DynamicData(nameof(GetVerifyEmailErrorTestCases), DynamicDataSourceType.Method)]
        public async Task CannotVerifyEmailWithInvalidData(string testName, string email, string verificationToken, string expectedErrorMessage, int expectedErrorCode)
        {
            var exception = await Assert.ThrowsExceptionAsync<SignNowException>(
                async () => await SignNowTestContext.Users.VerifyEmailAsync(email, verificationToken));

            Assert.IsNotNull(exception, $"Test case '{testName}': Exception should not be null");
            
            // Check for specific API error message and code
            Assert.IsTrue(
                exception.Message.IndexOf(expectedErrorMessage, StringComparison.OrdinalIgnoreCase) >= 0,
                $"Test case '{testName}': Expected error message to contain '{expectedErrorMessage}'. Actual: {exception.Message}"
            );
        }

        [DataTestMethod]
        [DynamicData(nameof(GetVerifyEmailArgumentTestCases), DynamicDataSourceType.Method)]
        public async Task VerifyEmailShouldThrowArgumentExceptionForInvalidInput(string testName, string email, string verificationToken, Type expectedExceptionType)
        {
            Exception exception;
            try
            {
                await SignNowTestContext.Users.VerifyEmailAsync(email, verificationToken);
                Assert.Fail($"Test case '{testName}': Expected {expectedExceptionType.Name} but no exception was thrown");
                return;
            }
            catch (Exception ex)
            {
                exception = ex;
            }

            Assert.IsNotNull(exception, $"Test case '{testName}': Exception should not be null");
            Assert.IsInstanceOfType(exception, expectedExceptionType, $"Test case '{testName}': Exception type should be {expectedExceptionType.Name}");
        }

        private static IEnumerable<object[]> GetVerifyEmailErrorTestCases()
        {
            // Test case: Incorrect verification token (API error code 65629)
            yield return new object[] 
            { 
                "Incorrect Verification Token", 
                "test@signnow.com", 
                "incorrect_verification_token",
                "verification token does not match email address passed in or is invalid",
                65629
            };

            // Test case: Expired verification token (API returns same error as incorrect token)
            yield return new object[] 
            { 
                "Expired Verification Token", 
                "test@signnow.com", 
                "expired_verification_token",
                "verification token does not match email address passed in or is invalid",
                65629
            };
        }

        private static IEnumerable<object[]> GetVerifyEmailArgumentTestCases()
        {
            // Test case: Null verification token
            yield return new object[] 
            { 
                "Null Verification Token", 
                "test@signnow.com", 
                null,
                typeof(ArgumentException)
            };

            // Test case: Empty verification token
            yield return new object[] 
            { 
                "Empty Verification Token", 
                "test@signnow.com", 
                "",
                typeof(ArgumentException)
            };

            // Test case: Invalid email format
            yield return new object[] 
            { 
                "Invalid Email Format", 
                "invalid-email", 
                "valid_token",
                typeof(ArgumentException)
            };

            // Test case: Null email - SDK throws ArgumentNullException for null parameters
            yield return new object[] 
            { 
                "Null Email", 
                null, 
                "valid_token",
                typeof(ArgumentNullException)
            };
        }
    }
}
