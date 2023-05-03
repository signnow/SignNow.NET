using System.Threading.Tasks;
using SignNow.Net.Model;
using SignNow.Net.Model.Requests;

namespace SignNow.Net.Examples.Users
{
    public static partial class UserExamples
    {
        /// <summary>
        /// Updates user information i.e. first name, last name
        /// </summary>
        /// <param name="firstname">new User firstname</param>
        /// <param name="lastname">new User lastname</param>
        /// <param name="oldPwd">Old User password</param>
        /// <param name="pwd">New User password</param>
        /// <param name="signNowContext">signNow container with services.</param>
        /// <returns></returns>
        public static async Task<UserUpdateResponse> ChangeUserDetails(string firstname, string lastname, string oldPwd, string pwd, SignNowContext signNowContext)
        {
            var userUpdateOptions = new UpdateUserOptions
            {
                FirstName = firstname,
                LastName = lastname,
                OldPassword = oldPwd,
                Password = pwd
            };

            return await signNowContext.Users.UpdateUserAsync(userUpdateOptions)
                .ConfigureAwait(false);
        }

        /// <summary>
        /// Sends password reset link via email example
        /// </summary>
        /// <param name="email">User email</param>
        /// <param name="signNowContext">signNow container with services.</param>
        /// <returns></returns>
        public static async Task SendsPasswordResetLink(string email, SignNowContext signNowContext)
        {
            await signNowContext.Users.SendPasswordResetLinkAsync(email)
                .ConfigureAwait(false);
        }
    }
}
