using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net.Model.Requests;

namespace SignNow.Net.Examples
{
    public partial class UserExamples
    {
        [TestMethod]
        public async Task ChangeUserDetailsAsync()
        {
            // Get current user details
            var currentUser = await testContext.Users
                .GetCurrentUserAsync()
                .ConfigureAwait(false);

            Console.WriteLine("Current user is: '{0}' '{1}'", currentUser.FirstName, currentUser.LastName);

            // Update user details
            var update = new UpdateUserOptions
            {
                FirstName = "signNow",
                LastName = currentUser.LastName + "Updated",
                OldPassword = credentials.Password,
                Password = credentials.Password,
                LogOutAll = false
            };

            var updateUserDetails = await testContext.Users
                .UpdateUserAsync(update)
                .ConfigureAwait(false);

            // get updated user details
            var updatedUser = await testContext.Users
                .GetCurrentUserAsync()
                .ConfigureAwait(false);

            Console.WriteLine("Updated user is: '{0}' '{1}'", updatedUser.FirstName, updatedUser.LastName);

            // check if user details were updated
            Assert.AreEqual(currentUser.FirstName, updateUserDetails.FirstName);
            Assert.AreNotEqual(currentUser.LastName, updateUserDetails.LastName);
            Assert.AreEqual(currentUser.LastName + "Updated", updateUserDetails.LastName);

            // Revert user details back
            var revert = new UpdateUserOptions
            {
                FirstName = currentUser.FirstName,
                LastName = currentUser.LastName,
                OldPassword = credentials.Password,
                Password = credentials.Password,
                LogOutAll = false
            };

            var revertedUser = await testContext.Users
                .UpdateUserAsync(revert)
                .ConfigureAwait(false);

            // get last user details
            var lastUserInfo = await testContext.Users
                .GetCurrentUserAsync()
                .ConfigureAwait(false);

            Console.WriteLine("Reverted user is: '{0}' '{1}'", lastUserInfo.FirstName, lastUserInfo.LastName);

            // check if user details were reverted
            Assert.AreEqual(currentUser.FirstName, lastUserInfo.FirstName);
            Assert.AreEqual(currentUser.LastName, lastUserInfo.LastName);
            Assert.AreEqual(currentUser.LastName, revertedUser.LastName);

            // send user password reset email
            await testContext.Users
                .SendPasswordResetLinkAsync(currentUser.Email)
                .ConfigureAwait(false);
        }
    }
}
