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
        /// <param name="token">Access token</param>
        /// <returns>
        /// Response with: User identity, email
        /// </returns>
        public static async Task<UserCreateResponse> CreateSignNowUser(string firstname, string lastname, string email, string password, Token token)
        {
            var signNowContext = new SignNowContext(token);

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
        /// <param name="token">Access token</param>
        /// <returns></returns>
        public static async Task<User> RetrieveUserInformation(Token token)
        {
            var signNowContext = new SignNowContext(token);

            return await signNowContext.Users.GetCurrentUserAsync()
                .ConfigureAwait(false);
        }
    }
}
