using System.Threading.Tasks;
using SignNow.Net.Model;

namespace SignNow.Net.Examples.Users
{
    public static partial class UserExamples
    {
        /// <summary>
        /// Sends verification email to a user example
        /// </summary>
        /// <param name="email">User email</param>
        /// <param name="token">Access token</param>
        /// <returns></returns>
        public static async Task SendVerificationEmailToUser(string email, Token token)
        {
            var signNowContext = new SignNowContext(token);

            await signNowContext.Users
                .SendVerificationEmailAsync(email)
                .ConfigureAwait(false);
        }
    }
}
