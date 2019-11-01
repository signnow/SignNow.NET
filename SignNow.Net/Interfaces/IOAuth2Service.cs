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
        /// <param name="scope">Specify a <see cref="Scope" /> for token request</param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled</param>
        /// <returns><see cref="Token" /> object</returns>
        Task<Token> GetTokenAsync(string login, string password, Scope scope, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieve Access token by authorization code
        /// </summary>
        /// <param name="code">Authorization code</param>
        /// <param name="scope">Specify a <see cref="Scope" /> for token request</param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled</param>
        /// <returns><see cref="Token" /> object</returns>
        Task<Token> GetTokenAsync(string code, Scope scope, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieve Access token by User's refresh token
        /// </summary>
        /// <param name="token">User's access <see cref="Token" /></param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled</param>
        /// <returns><see cref="Token" /> object</returns>
        Task<Token> RefreshTokenAsync(Token token, CancellationToken cancellationToken = default);

        Uri GetAuthorizationUrl(Uri redirectUrl);

        //Task<bool> ValidateTokenAsync(Token token, CancellationToken cancellationToken = default);
    }
}