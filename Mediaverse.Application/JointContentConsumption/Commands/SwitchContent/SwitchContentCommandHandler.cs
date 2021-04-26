using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Mediaverse.Application.JointContentConsumption.Commands.SwitchContent.Dtos;
using Mediaverse.Domain.Common;
using Mediaverse.Domain.JointContentConsumption.Enums;
using Mediaverse.Domain.JointContentConsumption.Repositories;
using Microsoft.Extensions.Logging;

namespace Mediaverse.Application.JointContentConsumption.Commands.SwitchContent
{
    public class SwitchContentCommandHandler : IRequestHandler<SwitchContentCommand, ContentDto>
    {
        private readonly IRoomRepository _roomRepository;
        private readonly IPlaylistRepository _playlistRepository;
        private readonly IContentRepository _contentRepository;
        private readonly ILogger<SwitchContentCommandHandler> _logger;
        private readonly IMapper _mapper;

        public SwitchContentCommandHandler(
            IRoomRepository roomRepository,
            IPlaylistRepository playlistRepository,
            IContentRepository contentRepository,
            ILogger<SwitchContentCommandHandler> logger,
            IMapper mapper)
        {
            _roomRepository = roomRepository;
            _playlistRepository = playlistRepository;
            _contentRepository = contentRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<ContentDto> Handle(SwitchContentCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var room = await _roomRepository.GetAsync(request.RoomId, cancellationToken)
                           ?? throw new InvalidOperationException($"Room {request.RoomId.ToString()} does not exist");

                var playlist = await _playlistRepository.GetAsync(room.ActivePlaylistId, cancellationToken)
                               ?? throw new InvalidOperationException($"Playlist {room.ActivePlaylistId.ToString()} " +
                                                                      $"does not exist");

                var contentId = request.Direction == SwitchContentDirection.Next
                    ? playlist.PlayNextContent()
                    : playlist.PlayPreviousContent();

                var content = await _contentRepository.GetAsync(contentId, cancellationToken);
                return _mapper.Map<ContentDto>(content);
            }
            catch (InformativeException exception)
            {
                _logger.LogError(exception, $"Could not play next content from active playlist of " +
                                            $"room {request.RoomId.ToString()}");
                throw;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, $"Could not play next content from active playlist of " +
                                 $"room {request.RoomId.ToString()}");
                throw new InformativeException("Could not play next content from the playlist. Please retry");
            }
        }
    }
}