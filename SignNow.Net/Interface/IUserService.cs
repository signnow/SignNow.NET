using SignNow.Net.Model;
using System.Threading;
using System.Threading.Tasks;

namespace SignNow.Net.Interface
{
    public interface IUserService
    {
        Task<User> GetCurrentUserAsync(CancellationToken cancellationToken = default(CancellationToken));
        Task<CreateUserResponse> CreateUserAsync(CreateUserRequest request, CancellationToken cancellationToken = default(CancellationToken));
    }
}
