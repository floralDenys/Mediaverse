using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Mediaverse.Domain.JointContentConsumption.Repositories;
using Microsoft.Extensions.Logging;

namespace Mediaverse.Application.JointContentConsumption.Commands.CloseRoom
{
    public class CloseRoomCommandHandler : IRequestHandler<CloseRoomCommand>
    {
        private readonly IRoomRepository _roomRepository;
        private readonly ILogger<CloseRoomCommandHandler> _logger;

        public CloseRoomCommandHandler(
            IRoomRepository roomRepository,
            ILogger<CloseRoomCommandHandler> logger)
        {
            _roomRepository = roomRepository;
            _logger = logger;
        }
        
        public async Task<Unit> Handle(CloseRoomCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var room = await _roomRepository.GetAsync(request.RoomId, cancellationToken) 
                           ?? throw new ArgumentException("Room could not be found");

                if (room.Host.Profile.Id != request.MemberId)
                {
                    throw new InvalidOperationException("Member does not have permission");
                }

                await _roomRepository.DeleteAsync(request.RoomId, cancellationToken);
                
                return Unit.Value;
            }
            catch (Exception exception)
            {
                _logger.LogError($"Could not close room {request.RoomId.ToString()}", exception);
                throw new InvalidOperationException("Could not close room. Please retry");
            }
        }
    }
}