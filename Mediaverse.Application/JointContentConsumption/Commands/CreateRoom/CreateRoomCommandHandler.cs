using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Mediaverse.Application.Common.Services;
using Mediaverse.Application.JointContentConsumption.Commands.CreateRoom.Dtos;
using Mediaverse.Domain.JointContentConsumption.Entities;
using Mediaverse.Domain.JointContentConsumption.Repositories;
using Microsoft.Extensions.Logging;

namespace Mediaverse.Application.JointContentConsumption.Commands.CreateRoom
{
    public class CreateRoomCommandHandler : IRequestHandler<CreateRoomCommand, RoomDto>
    {
        private readonly IRoomRepository _roomRepository;
        private readonly IViewerRepository _viewerRepository;
        private readonly IGuidProvider _guidProvider;
        private readonly ILogger<CreateRoomCommandHandler> _logger;
        private readonly IMapper _mapper;

        public CreateRoomCommandHandler(
            IRoomRepository roomRepository,
            IViewerRepository viewerRepository,
            IGuidProvider guidProvider,
            ILogger<CreateRoomCommandHandler> logger,
            IMapper mapper)
        {
            _roomRepository = roomRepository;
            _viewerRepository = viewerRepository;
            _guidProvider = guidProvider;
            _logger = logger;
            _mapper = mapper;
        }
        
        public async Task<RoomDto> Handle(CreateRoomCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (string.IsNullOrEmpty(request.Name))
                {
                    throw new ArgumentException("Name is null or empty");
                }
                
                var host = await _viewerRepository.GetAsync(request.HostId, cancellationToken) 
                           ?? throw new ArgumentException($"Host {request.HostId.ToString()} could not be found");

                Guid generatedRoomId = _guidProvider.GetNewGuid();
                var room = new Room(generatedRoomId, request.Name, host);
                
                await _roomRepository.AddAsync(room, cancellationToken);
                
                return _mapper.Map<RoomDto>(room);
            }
            catch (Exception exception)
            {
                _logger.LogError($"Could not create room with name {request.Name} by user with ID " +
                                 $"{request.HostId.ToString()}", exception);
                throw new InvalidOperationException("Could not create room. Please retry");
            }
        }
    }
}