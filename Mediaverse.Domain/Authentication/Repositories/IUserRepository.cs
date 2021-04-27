using System;
using System.Threading;
using System.Threading.Tasks;
using Mediaverse.Domain.Authentication.Entities;

namespace Mediaverse.Domain.Authentication.Repositories
{
    public interface IUserRepository
    {
        Task<User> GetUserAsync(Guid userId, CancellationToken cancellationToken);
    }
}