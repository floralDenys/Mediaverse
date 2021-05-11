using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using AutoMapper;
using MediatR;
using Mediaverse.Application.JointContentConsumption.Common.Dtos;
using Mediaverse.Domain.Common;
using Mediaverse.Domain.JointContentConsumption.Repositories;
using Mediaverse.Domain.JointContentConsumption.ValueObjects;
using Microsoft.Extensions.Logging;

namespace Mediaverse.Application.JointContentConsumption.Commands.PlaySpecificContent
{
    public class PlaySpecificContentCommandHandler : IRequestHandler<PlaySpecificContentCommand, AffectedViewersDto>
    {
        private readonly IRoomRepository _roomRepository;
        private readonly IContentRepository _contentRepository;
        private readonly ILogger<PlaySpecificContentCommandHandler> _logger;
        private readonly IMapper _mapper;

        public PlaySpecificContentCommandHandler(
            IRoomRepository roomRepository,
            IContentRepository contentRepository,
            ILogger<PlaySpecificContentCommandHandler> logger,
            IMapper mapper)
        {
            _roomRepository = roomRepository;
            _contentRepository = contentRepository;
            _logger = logger;
            _mapper = mapper;
        }
        
        public async Task<AffectedViewersDto> Handle(PlaySpecificContentCommand request, CancellationToken cancellationToken)
        {
            try
            {
                using var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
                
                var room = await _roomRepository.GetAsync(request.RoomId, cancellationToken) 
                           ?? throw new ArgumentException("Room could not be found");

                var contentId = _mapper.Map<ContentId>(request.ContentId);
                var content = await _contentRepository.GetAsync(contentId, cancellationToken)
                    ?? throw new ArgumentException("Content could not be found");
                
                room.PlayContent(contentId);

                await _roomRepository.UpdateAsync(room, cancellationToken);
                
                transaction.Complete();

                var affectedViewers = room.Viewers.ToList();
                affectedViewers.Add(room.Host);
                
                return _mapper.Map<AffectedViewersDto>(affectedViewers);
            }
            catch (InformativeException exception)
            {
                _logger.LogWarning(exception, $"Could not play content {request.ContentId} in room {request.RoomId}");
                throw;
            }
            catch (Exception exception)
            {
                _logger.LogWarning(exception, $"Could not play content {request.ContentId} in room {request.RoomId}");
                throw new InformativeException("Could not play requested content. Please retry");
            }
        }
    }
}