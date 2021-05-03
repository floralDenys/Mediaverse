using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Mediaverse.Application.JointContentConsumption.Queries.GetPlaylist.Dtos;
using Mediaverse.Domain.Common;
using Mediaverse.Domain.JointContentConsumption.Repositories;
using Microsoft.Extensions.Logging;

namespace Mediaverse.Application.JointContentConsumption.Queries.GetAvailablePlaylists
{
    public class GetAvailablePlaylistsQueryHandler : IRequestHandler<GetAvailablePlaylistsQuery, IList<SelectablePlaylistDto>>
    {
        private readonly IViewerRepository _viewerRepository;
        private readonly IPlaylistRepository _playlistRepository;
        private readonly ILogger<GetAvailablePlaylistsQueryHandler> _logger;
        private readonly IMapper _mapper;

        public GetAvailablePlaylistsQueryHandler(
            IViewerRepository viewerRepository,
            IPlaylistRepository playlistRepository,
            ILogger<GetAvailablePlaylistsQueryHandler> logger,
            IMapper mapper)
        {
            _viewerRepository = viewerRepository;
            _playlistRepository = playlistRepository;
            _logger = logger;
            _mapper = mapper;
        }
        
        public async Task<IList<SelectablePlaylistDto>> Handle(GetAvailablePlaylistsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var host = await _viewerRepository.GetAsync(request.HostId, cancellationToken)
                           ?? throw new ArgumentException("Room could not be found");

                var availablePlaylists =
                    await _playlistRepository.GetAllByViewerAsync(host.Profile.Id, cancellationToken);
                return _mapper.Map<IList<SelectablePlaylistDto>>(availablePlaylists);
            }
            catch (InformativeException exception)
            {
                _logger.LogError(exception, $"Could not get available playlists for room {request.HostId.ToString()}");
                throw;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, $"Could not get available playlists for room {request.HostId.ToString()}");
                throw new InformativeException("Could not get available playlists. Please retry");
            }
        }
    }
}