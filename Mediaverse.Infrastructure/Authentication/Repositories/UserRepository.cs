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
    }
}