using System;
using System.Threading.Tasks;
using Mediaverse.Domain.Authentication.Entities;

namespace Mediaverse.Domain.Authentication.Repositories
{
    public interface IUserRepository
    {
        Task<User> GetUserAsync(Guid userId);
        Task SaveUserAsync(User user);
    }
}