using SignNow.Net.Model;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SignNow.Net.Interfaces
{
    public interface IOAuth2Service
    {
        Task<Token> GetTokenAsync(string login, string password, Scope scope, CancellationToken cancellationToken = default(CancellationToken));
        Task<Token> GetTokenAsync(string code, Scope scope, CancellationToken cancellationToken = default(CancellationToken));//TODO: https://pdffiller.atlassian.net/wiki/spaces/API2/pages/911769793/Grant+type+authorization+code
        Task<Token> RefreshTokenAsync(Token token, CancellationToken cancellationToken = default(CancellationToken));
        Task<Uri> GetAuthorizationUrlAsync(Scope scope, string redirectUrl, CancellationToken cancellationToken = default(CancellationToken));
        Task<bool> ValidateTokenAsync(Token token, CancellationToken cancellationToken = default(CancellationToken));
    }
}
