using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net.Model.Requests;

namespace SignNow.Net.Examples
{
    [TestClass]
    public partial class UserExamples : ExamplesBase
    {
        [TestMethod]
        public async Task CreateSignNowUserAsync()
        {
            var timestamp = (long)(DateTime.Now - UnixEpoch).TotalSeconds;
            var email = $"signnow.tutorial+create_user_test{timestamp}@gmail.com";

            var userRequest = new CreateUserOptions
            {
                Email = email,
                FirstName = "John",
                LastName = "Wick",
                Password = "password"
            };

            // Create a new user
            var createUserResponse = await testContext.Users
                .CreateUserAsync(userRequest)
                .ConfigureAwait(false);

            // Check if the user was created and not verified
            Assert.AreEqual(email, createUserResponse.Email);
            Assert.IsFalse(createUserResponse.Verified);

            // Finally - send verification email to User
            await testContext.Users
                .SendVerificationEmailAsync(email)
                .ConfigureAwait(false);
        }
    }
}
