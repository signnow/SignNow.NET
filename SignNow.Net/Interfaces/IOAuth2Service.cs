using SignNow.Net.Model;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SignNow.Net.Interfaces
{
    public interface IOAuth2Service
    {
        /// <summary>
        /// Retrieve Access token by user's login and password
        /// </summary>
        /// <param name="login">User's login</param>
        /// <param name="password">User's password</param>
        /// <param name="scope">Specify a scope for token request</param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled</param>
        /// <returns>Token-object</returns>
        Task<Token> GetTokenAsync(string login, string password, Scope scope = Scope.All, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Retrieve Access token by user's authorization code
        /// </summary>
        /// <param name="code">User's authorization code</param>
        /// <param name="scope">Specify a scope for token request</param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled</param>
        /// <returns>Token-object</returns>
        Task<Token> GetTokenAsync(string code, Scope scope = Scope.All, CancellationToken cancellationToken = default(CancellationToken));

        Task<Token> RefreshTokenAsync(Token token, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Create Authorization URL to retrieve user token using authorization code
        /// </summary>
        /// <param name="scope">Specify a scope for token request</param>
        /// <param name="redirectUrl">Authorization code will send as query parameter to this redirect url</param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled</param>
        /// <returns></returns>
        Task<Uri> GetAuthorizationUrlAsync(string redirectUrl, CancellationToken cancellationToken = default(CancellationToken));

        Task<bool> ValidateTokenAsync(Token token, CancellationToken cancellationToken = default(CancellationToken));
    }
}