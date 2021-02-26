using System;
using System.Threading;
using System.Threading.Tasks;
using Mediaverse.Domain.Authentication.Entities;
using Mediaverse.Domain.Authentication.Repositories;
using Mediaverse.Infrastructure.Common.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Mediaverse.Infrastructure.Authentication.Repositories
{
    public class UserRepositories : IUserRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public UserRepositories(ApplicationDbContext dbContext) => _dbContext = dbContext;
        
        public Task<User> GetUserAsync(Guid userId, CancellationToken cancellationToken) =>
            _dbContext.Users.FindAsync(userId, cancellationToken).AsTask();

        public Task<User> GetUserAsync(string email, CancellationToken cancellationToken) =>
            _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email, cancellationToken);

        public Task<int> SaveUserAsync(User user, CancellationToken cancellationToken)
        {
            _dbContext.Users.Update(user);
            return _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}