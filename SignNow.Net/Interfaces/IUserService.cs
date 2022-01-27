using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SignNow.Net.Model;
using SignNow.Net.Model.Requests;

namespace SignNow.Net.Interfaces
{
    /// <summary>
    /// Interface for any operations with a User in signNow;
    /// i.e. create a user, authenticate a user, retrieve user's documents etc.
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Creates an account for a user
        /// </summary>
        /// <param name="createUser">User personal data (firstname, lastname, email, password)</param>
        /// <param name="cancellation">Propagates notification that operations should be canceled</param>
        /// <returns></returns>
        Task<UserCreateResponse> CreateUserAsync(CreateUserOptions createUser, CancellationToken cancellation = default);

        /// <summary>
        /// Retrieve current user`s resource
        /// </summary>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled</param>
        /// <returns></returns>
        Task<User> GetCurrentUserAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates user information i.e. first name, last name
        /// </summary>
        /// <param name="updateUser">User personal data (firstname, lastname, password)</param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled</param>
        /// <returns></returns>
        Task<UserUpdateResponse> UpdateUserAsync(UpdateUserOptions updateUser, CancellationToken cancellationToken = default);

        /// <summary>
        /// Sends verification email to a user
        /// </summary>
        /// <param name="email">User email</param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled</param>
        /// <returns></returns>
        Task SendVerificationEmailAsync(string email, CancellationToken cancellationToken = default);

        /// <summary>
        /// Sends password reset link
        /// </summary>
        /// <param name="email">User email</param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled</param>
        /// <returns></returns>
        Task SendPasswordResetLinkAsync(string email, CancellationToken cancellationToken = default);

        /// <summary>
        /// Returns an enumerable of user's documents that have been modified
        /// (added fields, texts, signatures, etc.) in descending order by modified date
        /// </summary>
        /// <param name="perPage">How many document objects to display per page in response. By default, it's 15, maximum 100.</param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns></returns>
        Task<IEnumerable<SignNowDocument>> GetModifiedDocumentsAsync(int perPage = 15, CancellationToken cancellationToken = default);

        /// <summary>
        /// Returns an enumerable of user's documents that that have not been modified yet.
        /// </summary>
        /// <param name="perPage">How many document objects to display per page in response. By default, it's 15, maximum 100.</param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns></returns>
        Task<IEnumerable<SignNowDocument>> GetUserDocumentsAsync(int perPage = 15, CancellationToken cancellationToken = default);
    }
}
