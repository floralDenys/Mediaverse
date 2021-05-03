using System;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using AutoMapper;
using MediatR;
using Mediaverse.Application.JointContentConsumption.Common.Dtos;
using Mediaverse.Domain.Common;
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
                using var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
                
                var viewer = await _viewerRepository.GetAsync(request.ViewerId, cancellationToken)
                             ?? throw new ArgumentException("Viewer could not be found");

                var room = await _roomRepository.GetAsync(request.RoomToken, cancellationToken)
                           ?? throw new InformativeException("Room could not be found");

                room.Join(viewer);

                await _roomRepository.UpdateAsync(room, cancellationToken);

                transaction.Complete();
                
                return _mapper.Map<RoomDto>(room);
            }
            catch (InformativeException exception)
            {
                _logger.LogError(exception, $"Viewer {request.ViewerId.ToString()} could not join the " +
                                            $"room {request.RoomToken}");
                throw;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, $"Viewer {request.ViewerId.ToString()} could not join the " +
                                 $"room {request.RoomToken}");
                throw new InformativeException("Failed to join the room. Please retry");
            }
        }
    }
}