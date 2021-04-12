using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Mediaverse.Application.JointContentConsumption.Common.Dtos;
using Mediaverse.Domain.JointContentConsumption.Repositories;
using Mediaverse.Domain.JointContentConsumption.ValueObjects;
using Microsoft.Extensions.Logging;

namespace Mediaverse.Application.JointContentConsumption.Commands.AddContentToPlaylist
{
    public class AddContentToPlaylistCommandHandler : IRequestHandler<AddContentToPlaylistCommand, PlaylistDto>
    {
        private readonly IRoomRepository _roomRepository;
        private readonly IPlaylistRepository _playlistRepository;
        private readonly ILogger<AddContentToPlaylistCommandHandler> _logger;
        private readonly IMapper _mapper;

        public AddContentToPlaylistCommandHandler(
            IRoomRepository roomRepository,
            IPlaylistRepository playlistRepository,
            ILogger<AddContentToPlaylistCommandHandler> logger,
            IMapper mapper)
        {
            _roomRepository = roomRepository;
            _playlistRepository = playlistRepository;
            _logger = logger;
            _mapper = mapper;
        }
        
        public async Task<PlaylistDto> Handle(AddContentToPlaylistCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var room = await _roomRepository.GetAsync(request.CurrentRoomId, cancellationToken)
                    ?? throw new ArgumentException($"Room {request.CurrentRoomId.ToString()} could not be found");
                
                var playlist = await _playlistRepository.GetAsync(room.ActivePlaylistId, cancellationToken)
                    ?? throw new InvalidOperationException($"Playlist {room.ActivePlaylistId.ToString()} " +
                                                           $"could not be found");
                
                var contentId = _mapper.Map<ContentId>(request.ContentId);
                playlist.Add(contentId);
                await _playlistRepository.UpdateAsync(playlist, cancellationToken);

                return _mapper.Map<PlaylistDto>(playlist);
            }
            catch (Exception exception)
            {
                _logger.LogError($"Could not add content {request.ContentId} to active playlist of " +
                                 $"room {request.CurrentRoomId.ToString()}", exception);
                throw new InvalidOperationException("Could not add content to the playlist. Please retry");
            }
        }
    }
}