using System;
using System.Threading;
using System.Threading.Tasks;
using Mediaverse.Domain.Authentication.Enums;
using Mediaverse.Domain.Authentication.Repositories;
using Mediaverse.Domain.JointContentConsumption.Entities;
using Mediaverse.Domain.JointContentConsumption.Repositories;
using Mediaverse.Domain.JointContentConsumption.ValueObjects;
using Mediaverse.Infrastructure.Authentication.Repositories;
using Mediaverse.Infrastructure.Common.Persistence;

namespace Mediaverse.Infrastructure.JointContentConsumption.Repositories
{
    public class RoomRepository : IRoomRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;

        private readonly IUserRepository _userRepository;

        public RoomRepository(
            ApplicationDbContext applicationDbContext,
            UserRepository userRepository)
        {
            _applicationDbContext = applicationDbContext;
            _userRepository = userRepository;
        }

        public async Task<Room> GetAsync(Guid roomId, CancellationToken cancellationToken)
        {
            var roomDto = await _applicationDbContext.Rooms.FindAsync(roomId);

            var hostUser = await _userRepository.GetUserAsync(roomDto.Host, cancellationToken);
            var host = new Viewer(
                new UserProfile(hostUser.Id, hostUser.Nickname, hostUser.Type == UserType.Member));
            
            var room = new Room(roomDto.Id, roomDto.Name, host);
            
            throw new NotImplementedException();
        }

        public Task SaveAsync(Room room, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Room room, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}