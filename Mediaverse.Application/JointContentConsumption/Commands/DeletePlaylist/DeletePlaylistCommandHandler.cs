using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using AutoMapper;
using MediatR;
using Mediaverse.Application.Common.Services;
using Mediaverse.Application.JointContentConsumption.Common.Dtos;
using Mediaverse.Domain.Common;
using Mediaverse.Domain.JointContentConsumption.Entities;
using Mediaverse.Domain.JointContentConsumption.Repositories;
using Microsoft.Extensions.Logging;

namespace Mediaverse.Application.JointContentConsumption.Commands.DeletePlaylist
{
    public class DeletePlaylistCommandHandler : IRequestHandler<DeletePlaylistCommand, AffectedViewers>
    {
        private readonly IPlaylistRepository _playlistRepository;
        private readonly IViewerRepository _viewerRepository;
        private readonly IRoomRepository _roomRepository;
        private readonly IIdentifierProvider _identifierProvider;
        private readonly ILogger<DeletePlaylistCommandHandler> _logger;
        private readonly IMapper _mapper;

        public DeletePlaylistCommandHandler(
            IPlaylistRepository playlistRepository,
            IViewerRepository viewerRepository,
            IRoomRepository roomRepository,
            IIdentifierProvider identifierProvider,
            ILogger<DeletePlaylistCommandHandler> logger,
            IMapper mapper)
        {
            _playlistRepository = playlistRepository;
            _viewerRepository = viewerRepository;
            _roomRepository = roomRepository;
            _identifierProvider = identifierProvider;
            _logger = logger;
            _mapper = mapper;
        }
        
        public async Task<AffectedViewers> Handle(DeletePlaylistCommand request, CancellationToken cancellationToken)
        {
            try
            {
                using var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
                
                var room = await _roomRepository.GetAsync(request.RoomId, cancellationToken)
                           ?? throw new ArgumentException("Room could not be found");
                
                var playlist = await _playlistRepository.GetAsync(room.ActivePlaylistId.Value, cancellationToken)
                               ?? throw new ArgumentException("Playlist could not be found");

                var viewer = await _viewerRepository.GetAsync(request.MemberId, cancellationToken)
                             ?? throw new ArgumentException("Viewer could not be found");

                if (!playlist.Owner.Equals(viewer))
                {
                    throw new InformativeException("Playlist does not belong to you");
                }

                await _playlistRepository.DeleteAsync(playlist.Id, cancellationToken);

                var remainingPlaylists = await _playlistRepository.GetAllByViewerAsync(request.MemberId, cancellationToken);
                if (remainingPlaylists == null || remainingPlaylists.Count == 0)
                {
                    var newPlaylistId = _identifierProvider.GenerateGuid();
                    var newPlaylist = new Playlist(newPlaylistId, "Temporary", viewer);
                    
                    await _playlistRepository.AddAsync(newPlaylist, cancellationToken);
                    
                    room.UpdateSelectedPlaylist(newPlaylist);
                }
                else
                {
                    room.UpdateSelectedPlaylist(remainingPlaylists.First());
                }

                await _roomRepository.UpdateAsync(room, cancellationToken);
                
                transaction.Complete();

                var affectedViewers = room.Viewers.ToList();
                affectedViewers.Add(room.Host);
                
                return _mapper.Map<AffectedViewers>(affectedViewers);
            }
            catch (InformativeException exception)
            {
                _logger.LogError(exception, $"Could not delete playlist {request.RoomId.ToString()} by member " +
                                            $"{request.MemberId.ToString()}");
                throw;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, $"Could not delete playlist {request.RoomId.ToString()} by member " +
                                 $"{request.MemberId.ToString()}");
                throw new InformativeException("Could not delete playlist. Please retry");
            }
        }
    }
}