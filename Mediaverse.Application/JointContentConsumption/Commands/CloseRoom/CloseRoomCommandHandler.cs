using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Mediaverse.Domain.Common;
using Mediaverse.Domain.JointContentConsumption.Repositories;
using Microsoft.Extensions.Logging;

namespace Mediaverse.Application.JointContentConsumption.Commands.CloseRoom
{
    public class CloseRoomCommandHandler : IRequestHandler<CloseRoomCommand>
    {
        private readonly IRoomRepository _roomRepository;
        private readonly IPlaylistRepository _playlistRepository;
        private readonly ILogger<CloseRoomCommandHandler> _logger;

        public CloseRoomCommandHandler(
            IRoomRepository roomRepository,
            IPlaylistRepository playlistRepository,
            ILogger<CloseRoomCommandHandler> logger)
        {
            _roomRepository = roomRepository;
            _playlistRepository = playlistRepository;
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
                    throw new InformativeException("Member does not have permission");
                }

                if (room.IsPlaylistSelected)
                {
                    var activePlaylist = await _playlistRepository.GetAsync(room.ActivePlaylistId.Value, cancellationToken)
                                         ?? throw new ArgumentException("Active playlist could not be found");

                    if (activePlaylist.IsTemporary)
                    {
                        await _playlistRepository.DeleteAsync(activePlaylist.Id, cancellationToken);
                    }
                }

                await _roomRepository.DeleteAsync(room.Id, cancellationToken);

                return Unit.Value;
            }
            catch (InformativeException exception)
            {
                _logger.LogError(exception, $"Could not close room {request.RoomId.ToString()}");
                throw;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, $"Could not close room {request.RoomId.ToString()}");
                throw new InformativeException("Could not close room. Please retry");
            }
        }
    }
}