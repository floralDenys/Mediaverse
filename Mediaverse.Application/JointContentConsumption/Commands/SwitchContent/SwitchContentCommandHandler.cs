using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using AutoMapper;
using MediatR;
using Mediaverse.Application.JointContentConsumption.Common.Dtos;
using Mediaverse.Domain.Common;
using Mediaverse.Domain.JointContentConsumption.Enums;
using Mediaverse.Domain.JointContentConsumption.Repositories;
using Mediaverse.Domain.JointContentConsumption.ValueObjects;
using Microsoft.Extensions.Logging;

namespace Mediaverse.Application.JointContentConsumption.Commands.SwitchContent
{
    public class SwitchContentCommandHandler : IRequestHandler<SwitchContentCommand, AffectedViewersDto>
    {
        private readonly IRoomRepository _roomRepository;
        private readonly IPlaylistRepository _playlistRepository;
        private readonly ILogger<SwitchContentCommandHandler> _logger;
        private readonly IMapper _mapper;

        public SwitchContentCommandHandler(
            IRoomRepository roomRepository,
            IPlaylistRepository playlistRepository,
            ILogger<SwitchContentCommandHandler> logger,
            IMapper mapper)
        {
            _roomRepository = roomRepository;
            _playlistRepository = playlistRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<AffectedViewersDto> Handle(SwitchContentCommand request, CancellationToken cancellationToken)
        {
            try
            {
                using var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
                
                var room = await _roomRepository.GetAsync(request.RoomId, cancellationToken)
                           ?? throw new InvalidOperationException($"Room {request.RoomId.ToString()} does not exist");

                var playlist = await _playlistRepository.GetAsync(room.ActivePlaylistId.Value, cancellationToken)
                               ?? throw new InvalidOperationException($"Playlist {room.ActivePlaylistId.ToString()} " +
                                                                      $"does not exist");

                ContentId contentId;
                if (playlist.Contains(room.CurrentContent?.ContentId))
                {
                    contentId = request.Direction == SwitchContentDirection.Next
                        ? playlist.GetNextContent(room.CurrentContent?.ContentId)
                        : playlist.GetPreviousContent(room.CurrentContent?.ContentId);
                }
                else
                {
                    contentId = playlist.FirstOrDefault()?.ContentId;
                }

                room.CurrentContent = new CurrentContent(
                    contentId,
                    ContentPlayerState.Paused,
                    playingTime: 0,
                    lastUpdatedPlayingTime: DateTime.Now);

                await _roomRepository.UpdateAsync(room, cancellationToken);

                transaction.Complete();

                var affectedViewers = room.Viewers.ToList();
                affectedViewers.Add(room.Host);
                
                return _mapper.Map<AffectedViewersDto>(affectedViewers);
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