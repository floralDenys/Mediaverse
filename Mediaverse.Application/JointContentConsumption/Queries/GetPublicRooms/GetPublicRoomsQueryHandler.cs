using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Mediaverse.Application.JointContentConsumption.Common.Dtos;
using Mediaverse.Domain.Common;
using Mediaverse.Domain.JointContentConsumption.Enums;
using Mediaverse.Domain.JointContentConsumption.Repositories;
using Microsoft.Extensions.Logging;

namespace Mediaverse.Application.JointContentConsumption.Queries.GetPublicRooms
{
    public class GetPublicRoomsQueryHandler : IRequestHandler<GetPublicRoomsQuery, IEnumerable<RoomDto>>
    {
        private readonly IRoomRepository _roomRepository;
        private readonly ILogger<GetPublicRoomsQueryHandler> _logger;
        private readonly IMapper _mapper;
        
        public GetPublicRoomsQueryHandler(
            IRoomRepository roomRepository,
            ILogger<GetPublicRoomsQueryHandler> logger,
            IMapper mapper)
        {
            _roomRepository = roomRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public Task<IEnumerable<RoomDto>> Handle(GetPublicRoomsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var rooms = _roomRepository.GetRooms(RoomType.Public, cancellationToken);
                return Task.FromResult(_mapper.Map<IEnumerable<RoomDto>>(rooms));
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Could not get public rooms");
                throw new InformativeException("Could not load public rooms. Please retry");
            }
        }
    }
}