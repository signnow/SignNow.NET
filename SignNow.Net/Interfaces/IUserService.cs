using SignNow.Net.Model;
using System.Threading;
using System.Threading.Tasks;

namespace SignNow.Net.Interfaces
{
    public interface IUserService
    {
        Task<User> GetCurrentUserAsync(CancellationToken cancellationToken = default(CancellationToken));
        Task<CreateUserResponse> CreateUserAsync(CreateUserRequest request, CancellationToken cancellationToken = default(CancellationToken));
    }
}
