using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
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

        private readonly IViewerRepository _viewerRepository;

        private readonly IMapper _mapper;
        
        public RoomRepository(
            ApplicationDbContext applicationDbContext,
            IViewerRepository viewerRepository, 
            IMapper mapper)
        {
            _applicationDbContext = applicationDbContext;
            _viewerRepository = viewerRepository;
            _mapper = mapper;
        }

        public async Task<Room> GetAsync(Guid roomId, CancellationToken cancellationToken)
        {
            var roomDto = await _applicationDbContext.Rooms.FindAsync(roomId);

            var host = await _viewerRepository.GetAsync(roomDto.HostId, cancellationToken);
            var viewers = (roomDto.Viewers ?? new List<ViewerDto>())
                .Select(x => _viewerRepository.GetAsync(x.Id, cancellationToken))
                .Select(t => t.Result)
                .ToList();
            
            return new Room(
                roomDto.Id,
                roomDto.Name,
                roomDto.Description,
                host, 
                roomDto.Type,
                new Invitation(roomDto.Token), 
                roomDto.MaxViewersQuantity,
                roomDto.ActivePlaylistId,
                viewers);
        }

        public async Task<Room> GetAsync(string roomToken, CancellationToken cancellationToken)
        {
            var roomDto = _applicationDbContext.Rooms.First(r => r.Token.Equals(roomToken));
            
            var host = await _viewerRepository.GetAsync(roomDto.HostId, cancellationToken);
            var viewers = roomDto.Viewers
                .Select(x => _viewerRepository.GetAsync(x.Id, cancellationToken))
                .Select(t => t.Result)
                .ToList();
            
            return new Room(
                roomDto.Id,
                roomDto.Name,
                roomDto.Description,
                host, 
                roomDto.Type,
                new Invitation(roomDto.Token), 
                roomDto.MaxViewersQuantity,
                roomDto.ActivePlaylistId,
                viewers);
        }

        public Task AddAsync(Room room, CancellationToken cancellationToken)
        {
            var roomDto = _mapper.Map<RoomDto>(room);

            _applicationDbContext.Rooms.Add(roomDto);
            
            return _applicationDbContext.SaveChangesAsync(cancellationToken);
        }

        public Task UpdateAsync(Room room, CancellationToken cancellationToken)
        {
            var roomDto = _applicationDbContext.Rooms.Find(room.Id);
            roomDto.Name = room.Name;
            roomDto.Description = room.Description;
            roomDto.HostId = room.Host.Profile.Id;
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
    }
}