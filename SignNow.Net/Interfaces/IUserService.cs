using System.Threading;
using System.Threading.Tasks;
using SignNow.Net.Model;

namespace SignNow.Net.Interfaces
{
    /// <summary>
    /// Represents a user resource
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