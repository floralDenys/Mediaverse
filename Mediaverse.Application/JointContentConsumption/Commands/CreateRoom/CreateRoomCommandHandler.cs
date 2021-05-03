using System;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using AutoMapper;
using MediatR;
using Mediaverse.Application.Common.Services;
using Mediaverse.Application.JointContentConsumption.Common.Dtos;
using Mediaverse.Domain.Common;
using Mediaverse.Domain.JointContentConsumption.Entities;
using Mediaverse.Domain.JointContentConsumption.Repositories;
using Microsoft.Extensions.Logging;

namespace Mediaverse.Application.JointContentConsumption.Commands.CreateRoom
{
    public class CreateRoomCommandHandler : IRequestHandler<CreateRoomCommand, RoomDto>
    {
        private readonly IRoomRepository _roomRepository;
        private readonly IViewerRepository _viewerRepository;
        private readonly IPlaylistRepository _playlistRepository;
        private readonly IIdentifierProvider _identifierProvider;
        private readonly ILogger<CreateRoomCommandHandler> _logger;
        private readonly IMapper _mapper;

        public CreateRoomCommandHandler(
            IRoomRepository roomRepository,
            IViewerRepository viewerRepository,
            IPlaylistRepository playlistRepository,
            IIdentifierProvider identifierProvider,
            ILogger<CreateRoomCommandHandler> logger,
            IMapper mapper)
        {
            _roomRepository = roomRepository;
            _viewerRepository = viewerRepository;
            _playlistRepository = playlistRepository;
            _identifierProvider = identifierProvider;
            _logger = logger;
            _mapper = mapper;
        }
        
        public async Task<RoomDto> Handle(CreateRoomCommand request, CancellationToken cancellationToken)
        {
            try
            {
                using var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
                
                var host = await _viewerRepository.GetAsync(request.HostId, cancellationToken)
                           ?? throw new ArgumentException($"Host {request.HostId.ToString()} could not be found");

                if (request.PlaylistId == default)
                {
                    Guid generatedPlaylistId = _identifierProvider.GenerateGuid();
                    var playlist = new Playlist(generatedPlaylistId, "Temporary", host)  {IsTemporary = true};
                    
                    await _playlistRepository.AddAsync(playlist, cancellationToken);
                    request.PlaylistId = generatedPlaylistId;
                }

                Guid generatedRoomId = _identifierProvider.GenerateGuid();
                string invitationToken = _identifierProvider.GenerateToken(generatedRoomId);

                var room = new Room(
                    generatedRoomId,
                    request.Name,
                    host,
                    request.Type,
                    new Invitation(invitationToken),
                    request.PlaylistId,
                    request.Description)
                {
                    MaxViewersQuantity = request.MaxViewersQuantity
                };
                    

                await _roomRepository.AddAsync(room, cancellationToken);

                transaction.Complete();
                
                return _mapper.Map<RoomDto>(room);
            }
            catch (InformativeException exception)
            {
                _logger.LogError(exception, $"Could not create room with name {request.Name} by user with ID " +
                                            $"{request.HostId.ToString()}");
                throw;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, $"Could not create room with name {request.Name} by user with ID " +
                                 $"{request.HostId.ToString()}");
                throw new InformativeException("Could not create room. Please retry");
            }
        }
    }
}