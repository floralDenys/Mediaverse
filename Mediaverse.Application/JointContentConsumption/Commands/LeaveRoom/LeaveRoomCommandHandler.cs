using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Mediaverse.Domain.JointContentConsumption.Repositories;
using Microsoft.Extensions.Logging;

namespace Mediaverse.Application.JointContentConsumption.Commands.LeaveRoom
{
    public class LeaveRoomCommandHandler : IRequestHandler<LeaveRoomCommand>
    {
        private readonly IRoomRepository _roomRepository;
        private readonly IViewerRepository _viewerRepository;
        private readonly ILogger<LeaveRoomCommandHandler> _logger;
        private readonly IMapper _mapper;

        public LeaveRoomCommandHandler(
            IRoomRepository roomRepository,
            IViewerRepository viewerRepository,
            ILogger<LeaveRoomCommandHandler> logger,
            IMapper mapper)
        {
            _roomRepository = roomRepository;
            _viewerRepository = viewerRepository;
            _logger = logger;
            _mapper = mapper;
        }
        
        public async Task<Unit> Handle(LeaveRoomCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var viewer = await _viewerRepository.GetAsync(request.ViewerId, cancellationToken) 
                             ?? throw new ArgumentException("Viewer could not be found");

                var room = await _roomRepository.GetAsync(request.RoomId, cancellationToken)
                           ?? throw new ArgumentException("Room could not be found");
                
                room.Leave(viewer);

                return Unit.Value;
            }
            catch (Exception exception)
            {
                _logger.LogError($"Viewer {request.ViewerId.ToString()} could not leave " +
                                 $"the room {request.RoomId.ToString()}", exception);
                throw new InvalidOperationException("Failed to leave the room. Please retry");
            }
        }
    }
}