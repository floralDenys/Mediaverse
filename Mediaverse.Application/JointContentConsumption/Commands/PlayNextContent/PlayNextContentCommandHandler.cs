using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Mediaverse.Application.JointContentConsumption.Commands.PlayNextContent.Dtos;
using Mediaverse.Domain.JointContentConsumption.Repositories;
using Microsoft.Extensions.Logging;

namespace Mediaverse.Application.JointContentConsumption.Commands.PlayNextContent
{
    public class PlayNextContentCommandHandler : IRequestHandler<PlayNextContentCommand, ContentDto>
    {
        private readonly IRoomRepository _roomRepository;
        private readonly IPlaylistRepository _playlistRepository;
        private readonly IContentRepository _contentRepository;
        private readonly ILogger<PlayNextContentCommandHandler> _logger;
        private readonly IMapper _mapper;

        public PlayNextContentCommandHandler(
            IRoomRepository roomRepository,
            IPlaylistRepository playlistRepository,
            IContentRepository contentRepository,
            ILogger<PlayNextContentCommandHandler> logger,
            IMapper mapper)
        {
            _roomRepository = roomRepository;
            _playlistRepository = playlistRepository;
            _contentRepository = contentRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<ContentDto> Handle(PlayNextContentCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var room = await _roomRepository.GetAsync(request.RoomId, cancellationToken) 
                           ?? throw new InvalidOperationException($"Room {request.RoomId.ToString()} does not exist");

                var playlist = await _playlistRepository.GetAsync(room.ActivePlaylistId, cancellationToken)
                               ?? throw new InvalidOperationException($"Playlist {room.ActivePlaylistId.ToString()} " +
                                                                      $"does not exist");

                var content = playlist.PlayNextContent();
                return _mapper.Map<ContentDto>(content);
            }
            catch (Exception exception)
            {
                throw new InvalidOperationException("Could not play next content from the playlist. Please retry");
            }
        }
    }
}