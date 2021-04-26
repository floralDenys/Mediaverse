using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Mediaverse.Application.Common.Services;
using Mediaverse.Domain.Common;
using Mediaverse.Domain.JointContentConsumption.Entities;
using Mediaverse.Domain.JointContentConsumption.Repositories;
using Mediaverse.Domain.JointContentConsumption.ValueObjects;
using Microsoft.Extensions.Logging;

namespace Mediaverse.Application.JointContentConsumption.Commands.SavePlaylist
{
    public class SavePlaylistCommandHandler : IRequestHandler<SavePlaylistCommand>
    {
        private readonly IPlaylistRepository _playlistRepository;
        private readonly IViewerRepository _viewerRepository;
        private readonly IRoomRepository _roomRepository;
        private readonly IIdentifierProvider _identifierProvider;
        private readonly ILogger<SavePlaylistCommandHandler> _logger;
        private readonly IMapper _mapper;

        public SavePlaylistCommandHandler(
            IPlaylistRepository playlistRepository,
            IViewerRepository viewerRepository,
            IRoomRepository roomRepository,
            IIdentifierProvider identifierProvider,
            ILogger<SavePlaylistCommandHandler> logger,
            IMapper mapper)
        {
            _playlistRepository = playlistRepository;
            _viewerRepository = viewerRepository;
            _roomRepository = roomRepository;
            _identifierProvider = identifierProvider;
            _logger = logger;
            _mapper = mapper;
        }
        
        public async Task<Unit> Handle(SavePlaylistCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var room = await _roomRepository.GetAsync(request.RoomId, cancellationToken)
                           ?? throw new ArgumentException("Room could not be found");

                var viewer = await _viewerRepository.GetAsync(request.ViewerId, cancellationToken)
                             ?? throw new ArgumentException("Viewer could not be found");

                if (!room.Viewers.Contains(viewer))
                {
                    throw new InvalidOperationException("Viewer does not belong to this room");
                }

                var activePlaylist = await _playlistRepository.GetAsync(room.ActivePlaylistId.Value, cancellationToken)
                                     ?? throw new ArgumentException("Playlist could not be found");

                if (activePlaylist.Owner == viewer)
                {
                    if (!activePlaylist.IsTemporary)
                    {
                        throw new InvalidOperationException("Playlist is added already");
                    }

                    activePlaylist.IsTemporary = false;
                }
                else
                {
                    Guid newPlaylistId = _identifierProvider.GenerateGuid();
                    activePlaylist = CreatePlaylistCopy(newPlaylistId, viewer, activePlaylist);
                }

                await _playlistRepository.AddAsync(activePlaylist, cancellationToken);

                return Unit.Value;
            }
            catch (InformativeException exception)
            {
                _logger.LogError(exception, $"Could not add playlist {request.RoomId.ToString()} " +
                                            $"to host of room {request.RoomId.ToString()}");
                throw;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, $"Could not add playlist {request.RoomId.ToString()} " +
                                 $"to host of room {request.RoomId.ToString()}");
                throw new InformativeException("Could not add playlist. Please retry");
            }
        }

        private Playlist CreatePlaylistCopy(Guid newPlaylistId, Viewer newPlaylistOwner, Playlist sourcePlaylist) =>
            new Playlist(newPlaylistId, newPlaylistOwner, sourcePlaylist);
    }
}