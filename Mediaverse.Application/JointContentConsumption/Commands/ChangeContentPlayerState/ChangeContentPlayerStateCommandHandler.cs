using System;
using System.Threading;
using System.Threading.Tasks;
using Lib.AspNetCore.ServerSentEvents;
using MediatR;
using Mediaverse.Domain.JointContentConsumption.Repositories;
using Microsoft.Extensions.Logging;

namespace Mediaverse.Application.JointContentConsumption.Commands.ChangeContentPlayerState
{
    public class PlayContentCommandHandler : IRequestHandler<ChangeContentPlayerStateCommand>
    {
        private readonly IRoomRepository _roomRepository;
        
        private readonly IServerSentEventsService _serverSentEventsService;

        private readonly ILogger<PlayContentCommandHandler> _logger;

        public PlayContentCommandHandler(
            IRoomRepository roomRepository,
            IServerSentEventsService serverSentEventsService,
            ILogger<PlayContentCommandHandler> logger)
        {
            _roomRepository = roomRepository;
            _serverSentEventsService = serverSentEventsService;
            _logger = logger;
        }
        
        public async Task<Unit> Handle(ChangeContentPlayerStateCommand request, CancellationToken cancellationToken)
        {
            try
            {
                return Unit.Value;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.ToString());
                throw;
            }
        }
    }
}