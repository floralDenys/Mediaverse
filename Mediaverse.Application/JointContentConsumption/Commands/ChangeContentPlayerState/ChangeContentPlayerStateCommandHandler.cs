using System;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using MediatR;
using Mediaverse.Domain.Common;
using Mediaverse.Domain.JointContentConsumption.Enums;
using Mediaverse.Domain.JointContentConsumption.Repositories;
using Microsoft.Extensions.Logging;

namespace Mediaverse.Application.JointContentConsumption.Commands.ChangeContentPlayerState
{
    public class PlayContentCommandHandler : IRequestHandler<ChangeContentPlayerStateCommand>
    {
        private readonly IRoomRepository _roomRepository;
        
        private readonly ILogger<PlayContentCommandHandler> _logger;

        public PlayContentCommandHandler(
            IRoomRepository roomRepository,
            ILogger<PlayContentCommandHandler> logger)
        {
            _roomRepository = roomRepository;
            _logger = logger;
        }
        
        public async Task<Unit> Handle(ChangeContentPlayerStateCommand request, CancellationToken cancellationToken)
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
                
                return Unit.Value;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.ToString());
                throw;
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