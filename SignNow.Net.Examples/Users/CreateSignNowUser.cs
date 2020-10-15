using System.Threading.Tasks;
using SignNow.Net.Model;
using SignNow.Net.Model.Requests;

namespace SignNow.Net.Examples.Users
{
    public static partial class UserExamples
    {
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
    }
}
