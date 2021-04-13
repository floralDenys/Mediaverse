using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Mediaverse.Application.JointContentConsumption.Common.Dtos;
using Mediaverse.Domain.JointContentConsumption.Repositories;
using Microsoft.Extensions.Logging;

namespace Mediaverse.Application.JointContentConsumption.Queries.GetAvailablePlaylists
{
    public class GetAvailablePlaylistsQueryHandler : IRequestHandler<GetAvailablePlaylistsQuery, IList<PlaylistDto>>
    {
        private readonly IRoomRepository _roomRepository;
        private readonly IPlaylistRepository _playlistRepository;
        private readonly ILogger<GetAvailablePlaylistsQueryHandler> _logger;
        private readonly IMapper _mapper;

        public GetAvailablePlaylistsQueryHandler(
            IRoomRepository roomRepository,
            IPlaylistRepository playlistRepository,
            ILogger<GetAvailablePlaylistsQueryHandler> logger,
            IMapper mapper)
        {
            _roomRepository = roomRepository;
            _playlistRepository = playlistRepository;
            _logger = logger;
            _mapper = mapper;
        }
        
        public async Task<IList<PlaylistDto>> Handle(GetAvailablePlaylistsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var room = await _roomRepository.GetAsync(request.RoomId, cancellationToken) 
                           ?? throw new ArgumentException("Room could not be found");

                var availablePlaylists = await _playlistRepository.GetAllByViewerAsync(room.Host.Profile.Id, cancellationToken);
                return _mapper.Map<IList<PlaylistDto>>(availablePlaylists);
            }
            catch (Exception exception)
            {
                _logger.LogError($"Could not get available playlists for room {request.RoomId.ToString()}", exception);
                throw new InvalidOperationException("Could not get available playlists. Please retry");
            }
        }
    }
}