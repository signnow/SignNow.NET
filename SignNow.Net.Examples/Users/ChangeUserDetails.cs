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

            // Update user details
            var update = new UpdateUserOptions
            {
                FirstName = currentUser.FirstName,
                LastName = currentUser.LastName + "Updated",
                OldPassword = credentials.Password,
                Password = credentials.Password
            };

            var updatedUser = await testContext.Users
                .UpdateUserAsync(update)
                .ConfigureAwait(false);

            // check if user details were updated
            Assert.AreEqual(currentUser.FirstName, updatedUser.FirstName);
            Assert.AreNotEqual(currentUser.LastName, updatedUser.LastName);
            Assert.AreEqual(currentUser.LastName + "Updated", updatedUser.LastName);

            // Revert user details back
            var revert = new UpdateUserOptions
            {
                FirstName = currentUser.FirstName,
                LastName = currentUser.LastName,
                OldPassword = credentials.Password,
                Password = credentials.Password
            };

            var revertedUser = await testContext.Users
                .UpdateUserAsync(revert)
                .ConfigureAwait(false);

            var lastUserInfo = await testContext.Users
                .GetCurrentUserAsync()
                .ConfigureAwait(false);

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
