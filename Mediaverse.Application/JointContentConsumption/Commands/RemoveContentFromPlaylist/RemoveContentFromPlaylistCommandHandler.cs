using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Mediaverse.Application.JointContentConsumption.Common.Dtos;
using Mediaverse.Domain.JointContentConsumption.Repositories;
using Mediaverse.Domain.JointContentConsumption.ValueObjects;
using Microsoft.Extensions.Logging;

namespace Mediaverse.Application.JointContentConsumption.Commands.RemoveContentFromPlaylist
{
    public class RemoveContentFromPlaylistCommandHandler : IRequestHandler<RemoveContentFromPlaylistCommand, PlaylistDto>
    {
        private readonly IRoomRepository _roomRepository;
        private readonly IPlaylistRepository _playlistRepository;
        private readonly ILogger<RemoveContentFromPlaylistCommandHandler> _logger;
        private readonly IMapper _mapper;

        public RemoveContentFromPlaylistCommandHandler(
            IRoomRepository roomRepository,
            IPlaylistRepository playlistRepository,
            ILogger<RemoveContentFromPlaylistCommandHandler> logger,
            IMapper mapper)
        {
            _roomRepository = roomRepository;
            _playlistRepository = playlistRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<PlaylistDto> Handle(RemoveContentFromPlaylistCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var room = await _roomRepository.GetAsync(request.CurrentRoomId, cancellationToken)
                    ?? throw new ArgumentException($"Room {request.CurrentRoomId.ToString()} could not be found");

                var activePlaylist = await _playlistRepository.GetAsync(room.ActivePlaylistId, cancellationToken)
                    ?? throw new InvalidOperationException($"Playlist {room.ActivePlaylistId.ToString()} " +
                                                           $"could not be found");

                var contentId = _mapper.Map<ContentId>(request.ContentId);
                activePlaylist.Remove(contentId);
                await _playlistRepository.UpdateAsync(activePlaylist, cancellationToken);

                return _mapper.Map<PlaylistDto>(activePlaylist);
            }
            catch (Exception exception)
            {
                _logger.LogError($"Could not remove content {request.ContentId} from active playlist of " +
                                 $"room {request.CurrentRoomId.ToString()}", exception);
                throw new InvalidOperationException("Could not remove content from the playlist. Please retry");
            }
        }
    }
}