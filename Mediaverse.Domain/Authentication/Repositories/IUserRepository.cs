using System;
using System.Threading;
using System.Threading.Tasks;
using Mediaverse.Domain.Authentication.Entities;

namespace Mediaverse.Domain.Authentication.Repositories
{
    public interface IUserRepository
    {
        Task<User> GetUserAsync(Guid userId, CancellationToken cancellationToken);
        Task<User> GetUserAsync(string email, CancellationToken cancellationToken);
        Task<int> AddUserAsync(User user, CancellationToken cancellationToken);
    }
}