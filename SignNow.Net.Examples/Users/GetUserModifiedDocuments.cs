using System.Collections.Generic;
using System.Threading.Tasks;
using SignNow.Net.Model;

namespace SignNow.Net.Examples.Users
{
    public static partial class UserExamples
    {
        /// <summary>
        /// Get User modified documents example
        /// </summary>
        /// <param name="perPage">How many document objects to display per page in response.</param>
        /// <param name="token">Access token.</param>
        /// <returns></returns>
        public static async Task<IEnumerable<SignNowDocument>> GetUserModifiedDocuments(int perPage, SignNowContext signNowContext)
        {
            return await signNowContext.Users
                .GetModifiedDocumentsAsync(perPage)
                .ConfigureAwait(false);
        }
    }
}
