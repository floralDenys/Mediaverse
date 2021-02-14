using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Mediaverse.Domain.JointContentConsumption.Repositories;
using Microsoft.Extensions.Logging;

namespace Mediaverse.Application.JointContentConsumption.Commands.AddPlaylist
{
    public class AddPlaylistCommandHandler : IRequestHandler<AddPlaylistCommand>
    {
        private readonly IPlaylistRepository _playlistRepository;
        private readonly IViewerRepository _viewerRepository;
        private readonly IRoomRepository _roomRepository;
        private readonly ILogger<AddPlaylistCommandHandler> _logger;
        private readonly IMapper _mapper;

        public AddPlaylistCommandHandler(
            IPlaylistRepository playlistRepository,
            IViewerRepository viewerRepository,
            IRoomRepository roomRepository,
            ILogger<AddPlaylistCommandHandler> logger,
            IMapper mapper)
        {
            _playlistRepository = playlistRepository;
            _viewerRepository = viewerRepository;
            _roomRepository = roomRepository;
            _logger = logger;
            _mapper = mapper;
        }
        
        public async Task<Unit> Handle(AddPlaylistCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var room = await _roomRepository.GetAsync(request.RoomId, cancellationToken)
                           ?? throw new ArgumentException("Room could not be found");

                var viewer = await _viewerRepository.GetViewerAsync(request.ViewerId, cancellationToken)
                             ?? throw new ArgumentException("Viewer could not be found");

                if (!room.Viewers.Contains(viewer))
                {
                    throw new InvalidOperationException("Viewer does not belong to this room");
                }
                
                var activePlaylist = await _playlistRepository.GetAsync(room.ActivePlaylistId, cancellationToken) 
                                     ?? throw new ArgumentException("Playlist could not be found");

                if (viewer == room.Host && activePlaylist.IsTemporary)
                {
                    activePlaylist.IsTemporary = false;
                }

                await _playlistRepository.SaveAsync(activePlaylist, cancellationToken);

                return Unit.Value;
            }
            catch (Exception exception)
            {
                _logger.LogError($"Could not add playlist {request.RoomId.ToString()} " +
                                 $"to host of room {request.RoomId.ToString()}", exception);
                throw new InvalidOperationException("Could not add playlist. Please retry");
            }
        }
    }
}