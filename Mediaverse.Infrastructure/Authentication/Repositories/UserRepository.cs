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
            _dbContext.Users.FindAsync(userId, cancellationToken).AsTask();

        public Task<User> GetUserAsync(string email, CancellationToken cancellationToken) =>
            _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email, cancellationToken);

        public async Task<int> SaveUserAsync(User user, CancellationToken cancellationToken)
        {
            var existingUser = await _dbContext.Users.FindAsync(user.Id);
            if (existingUser == null)
            {
                await _dbContext.Users.AddAsync(user, cancellationToken);
            }
            else
            {
                _dbContext.Users.Update(user);
            }
            
            return await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}