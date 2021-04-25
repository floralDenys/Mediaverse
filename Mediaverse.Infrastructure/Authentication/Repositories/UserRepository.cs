using System;
using System.Threading;
using System.Threading.Tasks;
using Mediaverse.Domain.Authentication.Entities;
using Mediaverse.Domain.Authentication.Repositories;
using Mediaverse.Infrastructure.Common.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Mediaverse.Infrastructure.Authentication.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public UserRepository(ApplicationDbContext dbContext) => _dbContext = dbContext;
        
        public Task<User> GetUserAsync(Guid userId, CancellationToken cancellationToken) =>
            _dbContext.Users.FindAsync(userId).AsTask();

        public Task<User> GetUserByEmailAsync(string email, CancellationToken cancellationToken) =>
            _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email, cancellationToken);

        public Task<User> GetUserByLoginAsync(string login, CancellationToken cancellationToken) =>
            _dbContext.Users.FirstOrDefaultAsync(u => u.Nickname == login, cancellationToken);

        public async Task<int> AddUserAsync(User user, CancellationToken cancellationToken)
        {
            await _dbContext.Users.AddAsync(user, cancellationToken);
            return await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}