using System.Threading.Tasks;
using SignNow.Net.Model;
using SignNow.Net.Model.Requests;

namespace SignNow.Net.Examples.Users
{
    public static partial class UserExamples
    {
        /// <summary>
        /// Creates an account for a user example
        /// </summary>
        /// <param name="firstname">User firstname</param>
        /// <param name="lastname">User lastname</param>
        /// <param name="email">User email</param>
        /// <param name="password">User password</param>
        /// <param name="signNowContext">signNow container with services.</param>
        /// <returns>
        /// Response with: User identity, email
        /// </returns>
        public static async Task<UserCreateResponse> CreateSignNowUser(string firstname, string lastname, string email, string password, SignNowContext signNowContext)
        {
            var userRequest = new CreateUserOptions
            {
                Email = email,
                FirstName = firstname,
                LastName = lastname,
                Password = password
            };

            return await signNowContext.Users
                .CreateUserAsync(userRequest)
                .ConfigureAwait(false);
        }

        /// <summary>
        /// Retrieve User Information example
        /// </summary>
        /// <param name="signNowContext">signNow container with services.</param>
        /// <returns></returns>
        public static async Task<User> RetrieveUserInformation(SignNowContext signNowContext)
        {
            return await signNowContext.Users.GetCurrentUserAsync()
                .ConfigureAwait(false);
        }
    }
}
