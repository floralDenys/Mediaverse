using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Mediaverse.Application.JointContentConsumption.Common.Dtos;
using Mediaverse.Domain.JointContentConsumption.Entities;
using Mediaverse.Domain.JointContentConsumption.Factories;
using Mediaverse.Domain.JointContentConsumption.Repositories;
using Microsoft.Extensions.Logging;

namespace Mediaverse.Application.JointContentConsumption.Commands.AddContentToPlaylist
{
    public class AddContentToPlaylistQueryHandler : IRequestHandler<AddContentToPlaylistQuery, PlaylistDto>
    {
        private readonly IRoomRepository _roomRepository;
        private readonly IPlaylistRepository _playlistRepository;
        private readonly ContentFactory _contentFactory;
        private readonly ILogger<AddContentToPlaylistQueryHandler> _logger;
        private readonly IMapper _mapper;

        public AddContentToPlaylistQueryHandler(
            IRoomRepository roomRepository,
            IPlaylistRepository playlistRepository,
            ContentFactory contentFactory,
            ILogger<AddContentToPlaylistQueryHandler> logger,
            IMapper mapper)
        {
            _roomRepository = roomRepository;
            _playlistRepository = playlistRepository;
            _contentFactory = contentFactory;
            _logger = logger;
            _mapper = mapper;
        }
        
        public async Task<PlaylistDto> Handle(AddContentToPlaylistQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var room = await _roomRepository.GetAsync(request.CurrentRoomId, cancellationToken)
                    ?? throw new ArgumentException($"Room {request.CurrentRoomId.ToString()} could not be found");

                Guid activePlaylistId = room.SelectedPlaylistId;
                var playlist = await _playlistRepository.GetAsync(activePlaylistId, cancellationToken)
                    ?? throw new InvalidOperationException($"Playlist {activePlaylistId.ToString()} could not be found");
                
                var contentId = _mapper.Map<ContentId>(request.ContentId);
                playlist.Add(_contentFactory.CreateContent(contentId, request.ContentType));
                await _playlistRepository.SaveAsync(playlist, cancellationToken);

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