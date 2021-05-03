using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Mediaverse.Domain.JointContentConsumption.Entities;
using Mediaverse.Domain.JointContentConsumption.Enums;
using Mediaverse.Domain.JointContentConsumption.Repositories;
using Mediaverse.Domain.JointContentConsumption.ValueObjects;
using Mediaverse.Infrastructure.Common.Persistence;
using Mediaverse.Infrastructure.JointContentConsumption.Repositories.Dtos;
using Microsoft.EntityFrameworkCore;

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

        public IEnumerable<Room> GetRooms(RoomType type, CancellationToken cancellationToken)
        {
            var rooms = _applicationDbContext.Rooms
                .Include(r => r.Viewers)
                .Where(r => r.Type == type).ToList();
            return rooms.Select(r => ConvertRoomAsync(r, cancellationToken))
                .Select(t => t.Result)
                .ToList();
        }

        public async Task<Room> GetAsync(Guid roomId, CancellationToken cancellationToken)
        {
            var roomDto = await _applicationDbContext.Rooms
                .Include(r => r.Viewers)
                .Include(r => r.CurrentContent)
                .FirstOrDefaultAsync(r => r.Id == roomId, cancellationToken);

            return await ConvertRoomAsync(roomDto, cancellationToken);
        }

        public async Task<Room> GetAsync(string roomToken, CancellationToken cancellationToken)
        {
            var roomDto = _applicationDbContext.Rooms
                .Include(r => r.Viewers)
                .Include(r => r.CurrentContent)
                .First(r => r.Token.Equals(roomToken));
            
            return await ConvertRoomAsync(roomDto, cancellationToken);
        }

        public Task AddAsync(Room room, CancellationToken cancellationToken)
        {
            var roomDto = _mapper.Map<RoomDto>(room);

            _applicationDbContext.Rooms.Add(roomDto);
            
            return _applicationDbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAsync(Room room, CancellationToken cancellationToken)
        {
            var roomDto = await _applicationDbContext.Rooms
                .Include(rd => rd.Viewers)
                .FirstOrDefaultAsync(rd => rd.Id == room.Id, cancellationToken);
            
            roomDto.Name = room.Name;
            roomDto.Description = room.Description;
            roomDto.HostId = room.Host.Profile.Id;
            roomDto.ActivePlaylistId = room.ActivePlaylistId;
            roomDto.MaxViewersQuantity = room.MaxViewersQuantity;

            if (room.CurrentContent != null && roomDto.CurrentContent != null
                && room.CurrentContent.ContentId.ExternalId == roomDto.CurrentContent.ExternalId
                && room.CurrentContent.ContentId.ContentSource == roomDto.CurrentContent.Source
                && room.CurrentContent.ContentId.ContentType == roomDto.CurrentContent.Type)
            {
                roomDto.CurrentContent.PlayerState = room.CurrentContent.PlayerState;
                roomDto.CurrentContent.PlayingTime = room.CurrentContent.PlayingTime;
                roomDto.CurrentContent.LastUpdatedPlayingTime = room.CurrentContent.LastUpdatedPlayingTime;
            }
            else
            {
                roomDto.CurrentContent = _mapper.Map<CurrentContentDto>(room.CurrentContent);
            }
            
            var addedViewers = room.Viewers
                .Where(v => roomDto.Viewers
                    .All(vd => vd.Id != v.Profile.Id))
                .Select(v =>
                    new ViewerDto
                    {
                        Id = v.Profile.Id,
                        RoomId = roomDto.Id
                    });
            roomDto.Viewers.AddRange(addedViewers);
            
            roomDto.Viewers.RemoveAll(vd => room.Viewers
                .All(v => v.Profile.Id != vd.Id));

            await _applicationDbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(Guid roomId, CancellationToken cancellationToken)
        {
            var roomDto = await _applicationDbContext.Rooms.FindAsync(roomId);
            _applicationDbContext.Remove(roomDto);

            await _applicationDbContext.SaveChangesAsync(cancellationToken);
        }

        private async Task<Room> ConvertRoomAsync(RoomDto roomDto, CancellationToken cancellationToken)
        {
            var host = await _viewerRepository.GetAsync(roomDto.HostId, cancellationToken);
            var viewers = roomDto.Viewers?
                .Select(x => _viewerRepository.GetAsync(x.Id, cancellationToken))
                .Select(t => t.Result)
                .ToList();

            var currentContent = _mapper.Map<CurrentContent>(roomDto.CurrentContent);
            
            return new Room(
                roomDto.Id,
                roomDto.Name,
                roomDto.Description,
                host, 
                roomDto.Type,
                new Invitation(roomDto.Token), 
                roomDto.MaxViewersQuantity,
                roomDto.ActivePlaylistId,
                viewers,
                currentContent);
        }
    }
}