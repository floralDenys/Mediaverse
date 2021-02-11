using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Mediaverse.Application.JointContentConsumption.Common.Dtos;
using Mediaverse.Domain.JointContentConsumption.Repositories;
using Microsoft.Extensions.Logging;

namespace Mediaverse.Application.JointContentConsumption.Commands.ChangeActivePlaylist
{
    public class ChangeActivePlaylistCommandHandler : IRequestHandler<ChangeActivePlaylistCommand, PlaylistDto>
    {
        private readonly IPlaylistRepository _playlistRepository;
        private readonly IRoomRepository _roomRepository;
        private readonly ILogger<ChangeActivePlaylistCommandHandler> _logger;
        private readonly IMapper _mapper;

        public ChangeActivePlaylistCommandHandler(
            IPlaylistRepository playlistRepository,
            IRoomRepository roomRepository,
            ILogger<ChangeActivePlaylistCommandHandler> logger, 
            IMapper mapper)
        {
            _playlistRepository = playlistRepository;
            _roomRepository = roomRepository;
            _logger = logger;
            _mapper = mapper;
        }
        
        public async Task<PlaylistDto> Handle(ChangeActivePlaylistCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var room = await _roomRepository.GetAsync(request.RoomId, cancellationToken) 
                           ?? throw new ArgumentException("Room could not be found");

                Guid previousPlaylistId = room.IsPlaylistSelected ? room.ActivePlaylistId : default;

                var newPlaylist = await _playlistRepository.GetAsync(request.PlaylistId, cancellationToken) 
                                  ?? throw new ArgumentException("New playlist could not be found");
                
                room.UpdateSelectedPlaylist(newPlaylist);
                
                if (previousPlaylistId != default)
                {
                    var previousPlaylist = await _playlistRepository.GetAsync(previousPlaylistId, cancellationToken)
                        ?? throw new InvalidOperationException("Previous active playlist could not be found");

                    if (previousPlaylist.IsTemporary)
                    {
                        await _playlistRepository.DeleteAsync(previousPlaylist, cancellationToken);
                    }
                }

                await _roomRepository.SaveAsync(room, cancellationToken);

                return _mapper.Map<PlaylistDto>(newPlaylist);
            }
            catch (Exception exception)
            {
                _logger.LogError($"Could not change active playlist in room {request.RoomId.ToString()}", exception);
                throw new InvalidOperationException("Could not change active playlist in room. Please retry");
            }
        }
    }
}