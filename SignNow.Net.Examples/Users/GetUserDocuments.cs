using System.Collections.Generic;
using System.Threading.Tasks;
using SignNow.Net.Model;

namespace SignNow.Net.Examples.Users
{
    public static partial class UserExamples
    {
        /// <summary>
        /// Get User documents that were not modified yet example
        /// </summary>
        /// <param name="perPage">How many document objects to display per page in response.</param>
        /// <param name="token">Access token.</param>
        /// <returns></returns>
        public static async Task<IEnumerable<SignNowDocument>> GetUserDocuments(int perPage, Token token)
        {
            var signNowContext = new SignNowContext(token);

            return await signNowContext.Users
                .GetUserDocumentsAsync(perPage)
                .ConfigureAwait(false);
        }
    }
}
