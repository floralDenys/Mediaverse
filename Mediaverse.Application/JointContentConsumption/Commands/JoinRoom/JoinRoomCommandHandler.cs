using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Mediaverse.Application.JointContentConsumption.Commands.CreateRoom.Dtos;
using Mediaverse.Domain.JointContentConsumption.Repositories;
using Microsoft.Extensions.Logging;

namespace Mediaverse.Application.JointContentConsumption.Commands.JoinRoom
{
    public class JoinRoomCommandHandler : IRequestHandler<JoinRoomCommand, RoomDto>
    {
        private readonly IRoomRepository _roomRepository;
        private readonly IViewerRepository _viewerRepository;
        private readonly ILogger<JoinRoomCommandHandler> _logger;
        private readonly IMapper _mapper;

        public JoinRoomCommandHandler(
            IRoomRepository roomRepository,
            IViewerRepository viewerRepository,
            ILogger<JoinRoomCommandHandler> logger,
            IMapper mapper)
        {
            _roomRepository = roomRepository;
            _viewerRepository = viewerRepository;
            _logger = logger;
            _mapper = mapper;
        }
        
        public async Task<RoomDto> Handle(JoinRoomCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var viewer = await _viewerRepository.GetAsync(request.ViewerId, cancellationToken)
                             ?? throw new ArgumentException("Viewer could not be found");

                var room = await _roomRepository.GetAsync(request.RoomId, cancellationToken)
                           ?? throw new ArgumentException("Room could not be found");

                if (!room.IsSpotAvailable)
                {
                    throw new InvalidOperationException("There is no spot for the viewer");
                }
                
                room.Join(viewer);

                await _roomRepository.UpdateAsync(room, cancellationToken);

                return _mapper.Map<RoomDto>(room);
            }
            catch (Exception exception)
            {
                _logger.LogError($"Viewer {request.ViewerId.ToString()} could not join the " +
                                 $"room {request.RoomId.ToString()}", exception);
                throw new InvalidOperationException("Failed to join the room. Please retry");
            }
        }
    }
}