using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using AutoMapper;
using MediatR;
using Mediaverse.Application.JointContentConsumption.Common.Dtos;
using Mediaverse.Domain.Common;
using Mediaverse.Domain.JointContentConsumption.Repositories;
using Mediaverse.Domain.JointContentConsumption.ValueObjects;
using Microsoft.Extensions.Logging;

namespace Mediaverse.Application.JointContentConsumption.Commands.AddContentToPlaylist
{
    public class AddContentToPlaylistCommandHandler : IRequestHandler<AddContentToPlaylistCommand, AffectedViewers>
    {
        private readonly IRoomRepository _roomRepository;
        private readonly IPlaylistRepository _playlistRepository;
        private readonly IContentRepository _contentRepository;
        private readonly ILogger<AddContentToPlaylistCommandHandler> _logger;
        private readonly IMapper _mapper;

        public AddContentToPlaylistCommandHandler(
            IRoomRepository roomRepository,
            IPlaylistRepository playlistRepository,
            IContentRepository contentRepository,
            ILogger<AddContentToPlaylistCommandHandler> logger,
            IMapper mapper)
        {
            _roomRepository = roomRepository;
            _playlistRepository = playlistRepository;
            _contentRepository = contentRepository;
            _logger = logger;
            _mapper = mapper;
        }
        
        public async Task<AffectedViewers> Handle(AddContentToPlaylistCommand request, CancellationToken cancellationToken)
        {
            try
            {
                using var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
                
                var room = await _roomRepository.GetAsync(request.CurrentRoomId, cancellationToken)
                           ?? throw new ArgumentException(
                               $"Room {request.CurrentRoomId.ToString()} could not be found");

                var playlist = await _playlistRepository.GetAsync(room.ActivePlaylistId.Value, cancellationToken)
                               ?? throw new InvalidOperationException(
                                   $"Playlist {room.ActivePlaylistId.ToString()} " +
                                   $"could not be found");

                var contentId = _mapper.Map<ContentId>(request.ContentId);
                await _contentRepository.GetAsync(contentId, cancellationToken);

                playlist.Add(contentId);
                await _playlistRepository.UpdateAsync(playlist, cancellationToken);

                transaction.Complete();
                    
                var affectedViewers = room.Viewers.ToList();
                affectedViewers.Add(room.Host);
                
                return _mapper.Map<AffectedViewers>(affectedViewers);
            }
            catch (InformativeException exception)
            {
                _logger.LogError(exception, $"Could not add content {request.ContentId} to active playlist of " +
                                            $"room {request.CurrentRoomId.ToString()}");
                throw;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, $"Could not add content {request.ContentId} to active playlist of " +
                                 $"room {request.CurrentRoomId.ToString()}");
                throw new InformativeException("Could not add content to the playlist. Please retry");
            }
        }
    }
}