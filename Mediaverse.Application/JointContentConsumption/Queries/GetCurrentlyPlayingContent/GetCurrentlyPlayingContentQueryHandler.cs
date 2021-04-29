using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Mediaverse.Application.JointContentConsumption.Commands.SwitchContent.Dtos;
using Mediaverse.Domain.Common;
using Mediaverse.Domain.JointContentConsumption.Repositories;
using Microsoft.Extensions.Logging;

namespace Mediaverse.Application.JointContentConsumption.Queries.GetCurrentlyPlayingContent
{
    public class GetCurrentlyPlayingContentQueryHandler : IRequestHandler<GetCurrentlyPlayingContentQuery, ContentDto>
    {
        private readonly IRoomRepository _roomRepository;
        private readonly IContentRepository _contentRepository;
        private readonly ILogger<GetCurrentlyPlayingContentQueryHandler> _logger;
        private readonly IMapper _mapper;

        public GetCurrentlyPlayingContentQueryHandler(
            IRoomRepository roomRepository,
            IContentRepository contentRepository,
            ILogger<GetCurrentlyPlayingContentQueryHandler> logger,
            IMapper mapper)
        {
            _roomRepository = roomRepository;
            _contentRepository = contentRepository;
            _logger = logger;
            _mapper = mapper;
        }
        
        public async Task<ContentDto> Handle(GetCurrentlyPlayingContentQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var room = await _roomRepository.GetAsync(request.RoomId, cancellationToken)
                           ?? throw new InformativeException("Room could not be found");

                if (room.CurrentContent == null)
                {
                    return new ContentDto();
                }

                var content = await _contentRepository.GetAsync(room.CurrentContent.ContentId, cancellationToken);
                var dto = _mapper.Map<ContentDto>(content);
                dto.Player.State = room.CurrentContent.PlayerState.ToString();
                dto.Player.PlayingTime = room.CurrentContent.PlayingTime;
                
                return dto;
            }
            catch (InformativeException exception)
            {
                _logger.LogWarning(exception, $"Could not get currently playing content in room {request.RoomId}");
                throw;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, $"Could not get currently playing content in room {request.RoomId}");
                throw new InformativeException("Could not get currently playing content. Please retry");
            }
        }
    }
}