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
using Microsoft.Extensions.Logging;

namespace Mediaverse.Application.JointContentConsumption.Commands.ChangeContentPlayerState
{
    public class PlayContentCommandHandler : IRequestHandler<ChangeContentPlayerStateCommand, AffectedViewersDto>
    {
        private readonly IRoomRepository _roomRepository;
        
        private readonly ILogger<PlayContentCommandHandler> _logger;

        private readonly IMapper _mapper;

        public PlayContentCommandHandler(
            IRoomRepository roomRepository,
            ILogger<PlayContentCommandHandler> logger,
            IMapper mapper)
        {
            _roomRepository = roomRepository;
            _logger = logger;
            _mapper = mapper;
        }
        
        public async Task<AffectedViewersDto> Handle(ChangeContentPlayerStateCommand request, CancellationToken cancellationToken)
        {
            try
            {
                using var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
                
                var room = await _roomRepository.GetAsync(request.RoomId, cancellationToken)
                    ?? throw new InformativeException("Could not find the room");

                room.CurrentContent.PlayerState = ConvertFromString(request.State);
                room.CurrentContent.PlayingTime = (long)request.CurrentPlaybackTimeInSeconds;
                room.CurrentContent.LastUpdatedPlayingTime = DateTime.Now;

                await _roomRepository.UpdateAsync(room, cancellationToken);

                transaction.Complete();

                var affectedViewers = room.Viewers.ToList();
                affectedViewers.Add(room.Host);
                
                return _mapper.Map<AffectedViewersDto>(affectedViewers);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, $"Could not change player state in room {request.RoomId.ToString()}");
                throw new InformativeException("Could not change player state. Please retry");
            }
        }

        private ContentPlayerState ConvertFromString(string state)
        {
            if (state == "playing")
            {
                return ContentPlayerState.Playing;
            }
            else
            {
                return ContentPlayerState.Paused;
            }
        }
    }
}