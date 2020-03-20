using System.Threading;
using System.Threading.Tasks;
using SignNow.Net.Model;

namespace SignNow.Net.Interfaces
{
    /// <summary>
    /// Interface for any operations with a User in SignNow; i.e. create a user, authenticate a user, retrieve user's documents etc.
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Retrieve current user`s resource
        /// </summary>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled</param>
        /// <returns></returns>
        Task<User> GetCurrentUserAsync(CancellationToken cancellationToken = default);
    }
}
