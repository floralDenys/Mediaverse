using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Mediaverse.Application.JointContentConsumption.Commands.SwitchContent.Dtos;
using Mediaverse.Application.JointContentConsumption.Common.Dtos;
using Mediaverse.Domain.Common;
using Mediaverse.Domain.JointContentConsumption.Entities;
using Mediaverse.Domain.JointContentConsumption.Repositories;
using Microsoft.Extensions.Logging;

namespace Mediaverse.Application.JointContentConsumption.Queries.GetRoom
{
    public class GetRoomQueryHandler : IRequestHandler<GetRoomQuery, RoomDto>
    {
        private readonly IRoomRepository _roomRepository;
        private readonly IPlaylistRepository _playlistRepository;
        private readonly IContentRepository _contentRepository;
        private readonly ILogger<GetRoomQueryHandler> _logger;
        private readonly IMapper _mapper;

        public GetRoomQueryHandler(
            IRoomRepository roomRepository,
            IPlaylistRepository playlistRepository,
            IContentRepository contentRepository,
            ILogger<GetRoomQueryHandler> logger,
            IMapper mapper)
        {
            _roomRepository = roomRepository;
            _playlistRepository = playlistRepository;
            _contentRepository = contentRepository;
            _logger = logger;
            _mapper = mapper;
            _contentRepository = contentRepository;
        }
        
        public async Task<RoomDto> Handle(GetRoomQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var room = await _roomRepository.GetAsync(request.RoomId, cancellationToken)
                    ?? throw new ArgumentException("Room could not be found");
                
                return _mapper.Map<RoomDto>(room);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, $"Could not get room {request.RoomId}");
                throw new InformativeException("Could not get room. Please retry");
            }
        }
    }
}