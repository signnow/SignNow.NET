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
        /// <param name="signNowContext">signNow container with services.</param>
        /// <returns></returns>
        public static async Task SendVerificationEmailToUser(string email, SignNowContext signNowContext)
        {
            await signNowContext.Users
                .SendVerificationEmailAsync(email)
                .ConfigureAwait(false);
        }
    }
}
