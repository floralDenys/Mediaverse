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

namespace Mediaverse.Application.JointContentConsumption.Commands.RemoveContentFromPlaylist
{
    public class RemoveContentFromPlaylistCommandHandler : IRequestHandler<RemoveContentFromPlaylistCommand, AffectedViewers>
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

        public async Task<AffectedViewers> Handle(RemoveContentFromPlaylistCommand request, CancellationToken cancellationToken)
        {
            try
            {
                using var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
                
                var room = await _roomRepository.GetAsync(request.CurrentRoomId, cancellationToken)
                           ?? throw new ArgumentException(
                               $"Room {request.CurrentRoomId.ToString()} could not be found");

                var activePlaylist = await _playlistRepository.GetAsync(room.ActivePlaylistId.Value, cancellationToken)
                                     ?? throw new InvalidOperationException(
                                         $"Playlist {room.ActivePlaylistId.ToString()} " +
                                         $"could not be found");

                var contentId = _mapper.Map<ContentId>(request.ContentId);

                if (contentId.Equals(room.CurrentContent?.ContentId))
                {
                    throw new InformativeException("Could not remove currently playing content.");
                }
                
                activePlaylist.Remove(contentId);
                await _playlistRepository.UpdateAsync(activePlaylist, cancellationToken);

                transaction.Complete();

                var affectedUsers = room.Viewers.ToList();
                affectedUsers.Add(room.Host);
                
                return _mapper.Map<AffectedViewers>(affectedUsers);
            }
            catch (InformativeException exception)
            {
                _logger.LogError(exception, $"Could not remove content {request.ContentId} from active playlist of " +
                                            $"room {request.CurrentRoomId.ToString()}");
                throw;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, $"Could not remove content {request.ContentId} from active playlist of " +
                                 $"room {request.CurrentRoomId.ToString()}");
                throw new InformativeException("Could not remove content from the playlist. Please retry");
            }
        }
    }
}