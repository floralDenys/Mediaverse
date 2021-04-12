using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Mediaverse.Domain.Authentication.Enums;
using Mediaverse.Domain.Authentication.Repositories;
using Mediaverse.Domain.JointContentConsumption.Entities;
using Mediaverse.Domain.JointContentConsumption.Repositories;
using Mediaverse.Domain.JointContentConsumption.ValueObjects;
using Mediaverse.Infrastructure.Authentication.Repositories;
using Mediaverse.Infrastructure.Common.Persistence;
using Mediaverse.Infrastructure.JointContentConsumption.Repositories.Dtos;

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

            var host = await GetViewer(roomDto.Host, cancellationToken);
            var viewers = roomDto.Viewers
                .Select(x => GetViewer(x.Id, cancellationToken))
                .Select(t => t.Result)
                .ToList();
            
            return new Room(
                roomDto.Id,
                roomDto.Name,
                host, 
                roomDto.MaxViewersQuantity,
                roomDto.ActivePlaylistId,
                viewers);
        }

        public Task AddAsync(Room room, CancellationToken cancellationToken)
        {
            var roomDto = new RoomDto
            {
                Id = room.Id,
                Name = room.Name,
                Host = room.Host.Profile.Id,
                ActivePlaylistId = room.ActivePlaylistId,
                MaxViewersQuantity = room.MaxViewersQuantity,
                Viewers = room.Viewers
                    .Select(v => new ViewerDto{ Id = v.Profile.Id })
                    .ToList()
            };

            _applicationDbContext.Rooms.Add(roomDto);
            
            return _applicationDbContext.SaveChangesAsync(cancellationToken);
        }

        public Task UpdateAsync(Room room, CancellationToken cancellationToken)
        {
            var roomDto = _applicationDbContext.Rooms.Find(room.Id);
            roomDto.Name = room.Name;
            roomDto.Host = room.Host.Profile.Id;
            roomDto.ActivePlaylistId = room.ActivePlaylistId;
            roomDto.MaxViewersQuantity = room.MaxViewersQuantity;
            roomDto.Viewers = room.Viewers
                .Select(v => new ViewerDto {Id = v.Profile.Id})
                .ToList();

            return _applicationDbContext.SaveChangesAsync(cancellationToken);
        }

        public Task DeleteAsync(Guid roomId, CancellationToken cancellationToken)
        {
            var roomDto = _applicationDbContext.Rooms.FindAsync(roomId);
            _applicationDbContext.Remove(roomDto);

            return _applicationDbContext.SaveChangesAsync(cancellationToken);
        }
        
        private async Task<Viewer> GetViewer(Guid viewerId, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserAsync(viewerId, cancellationToken);
            return new Viewer(new UserProfile(user.Id, user.Nickname, user.Type == UserType.Member));
        }
    }
}