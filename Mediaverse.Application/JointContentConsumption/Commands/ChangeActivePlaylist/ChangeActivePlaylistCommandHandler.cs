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
using Microsoft.Extensions.Logging;

namespace Mediaverse.Application.JointContentConsumption.Commands.ChangeActivePlaylist
{
    public class ChangeActivePlaylistCommandHandler : IRequestHandler<ChangeActivePlaylistCommand, AffectedViewersDto>
    {
        private readonly IPlaylistRepository _playlistRepository;
        private readonly IRoomRepository _roomRepository;
        private readonly ILogger<ChangeActivePlaylistCommandHandler> _logger;
        private readonly IMapper _mapper;

        public ChangeActivePlaylistCommandHandler(
            IPlaylistRepository playlistRepository,
            IRoomRepository roomRepository,
            ILogger<ChangeActivePlaylistCommandHandler> logger, 
            IMapper mapper)
        {
            _playlistRepository = playlistRepository;
            _roomRepository = roomRepository;
            _logger = logger;
            _mapper = mapper;
        }
        
        public async Task<AffectedViewersDto> Handle(ChangeActivePlaylistCommand request, CancellationToken cancellationToken)
        {
            try
            {
                using var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
                
                var room = await _roomRepository.GetAsync(request.RoomId, cancellationToken)
                           ?? throw new ArgumentException("Room could not be found");

                var newPlaylist = await _playlistRepository.GetAsync(request.PlaylistId, cancellationToken)
                                  ?? throw new ArgumentException("New playlist could not be found");

                room.UpdateSelectedPlaylist(newPlaylist);

                if (room.IsPlaylistSelected)
                {
                    var previousPlaylist = await _playlistRepository.GetAsync(room.ActivePlaylistId.Value, cancellationToken)
                                           ?? throw new InvalidOperationException(
                                               "Previous active playlist could not be found");

                    if (previousPlaylist.IsTemporary)
                    {
                        await _playlistRepository.DeleteAsync(previousPlaylist.Id, cancellationToken);
                    }
                }

                await _roomRepository.UpdateAsync(room, cancellationToken);

                transaction.Complete();

                var affectedViewers = room.Viewers.ToList();
                affectedViewers.Add(room.Host);
                
                return _mapper.Map<AffectedViewersDto>(affectedViewers);
            }
            catch (InformativeException exception)
            {
                _logger.LogError(exception, $"Could not change active playlist in room {request.RoomId.ToString()}");
                throw;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, $"Could not change active playlist in room {request.RoomId.ToString()}");
                throw new InformativeException("Could not change active playlist in room. Please retry");
            }
        }
    }
}