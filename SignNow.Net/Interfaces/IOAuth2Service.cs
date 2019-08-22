using SignNow.Net.Model;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SignNow.Net.Interfaces
{
    public interface IOAuth2Service
    {
        /// <summary>
        /// Retrieve Acccess token by client's login and password
        /// </summary>
        /// <param name="login">user's login</param>
        /// <param name="password">user's password</param>
        /// <param name="scope">parameter 'scope' for HHTP request</param>
        /// <param name="cancellationToken">stopping method's execution</param>
        /// <returns>Token-object</returns>
        Task<Token> GetTokenAsync(string login, string password, Scope scope, CancellationToken cancellationToken = default(CancellationToken));

        Task<Token> GetTokenAsync(string code, Scope scope, CancellationToken cancellationToken = default(CancellationToken));//TODO: https://pdffiller.atlassian.net/wiki/spaces/API2/pages/911769793/Grant+type+authorization+code

        Task<Token> RefreshTokenAsync(Token token, CancellationToken cancellationToken = default(CancellationToken));

        Task<Uri> GetAuthorizationUrlAsync(Scope scope, CancellationToken cancellationToken = default(CancellationToken));

        Task<bool> ValidateTokenAsync(Token token, CancellationToken cancellationToken = default(CancellationToken));
    }
}