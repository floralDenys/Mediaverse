using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Mediaverse.Application.JointContentConsumption.Common.Dtos;
using Mediaverse.Domain.Common;
using Mediaverse.Domain.JointContentConsumption.Repositories;
using Microsoft.Extensions.Logging;

namespace Mediaverse.Application.JointContentConsumption.Queries.GetPlaylist
{
    public class GetPlaylistQueryHandler : IRequestHandler<GetPlaylistQuery, PlaylistDto>
    {
        private readonly IPlaylistRepository _playlistRepository;
        private readonly ILogger<GetPlaylistQueryHandler> _logger;
        private readonly IMapper _mapper;

        public GetPlaylistQueryHandler(
            IPlaylistRepository playlistRepository,
            ILogger<GetPlaylistQueryHandler> logger,
            IMapper mapper)
        {
            _playlistRepository = playlistRepository;
            _logger = logger;
            _mapper = mapper;
        }
        
        public async Task<PlaylistDto> Handle(GetPlaylistQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var playlist = await _playlistRepository.GetAsync(request.PlaylistId, cancellationToken);
                return _mapper.Map<PlaylistDto>(playlist);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, $"Could not get playlist {request.PlaylistId}");
                throw new InformativeException("Could not get playlist. Please retry");
            }
        }
    }
}