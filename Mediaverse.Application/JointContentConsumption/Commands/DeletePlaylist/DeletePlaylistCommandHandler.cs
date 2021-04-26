using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Mediaverse.Application.JointContentConsumption.Common.Dtos;
using Mediaverse.Domain.Common;
using Mediaverse.Domain.JointContentConsumption.Repositories;
using Microsoft.Extensions.Logging;

namespace Mediaverse.Application.JointContentConsumption.Commands.DeletePlaylist
{
    public class DeletePlaylistCommandHandler : IRequestHandler<DeletePlaylistCommand, IList<PlaylistDto>>
    {
        private readonly IPlaylistRepository _playlistRepository;
        private readonly IViewerRepository _viewerRepository;
        private readonly ILogger<DeletePlaylistCommandHandler> _logger;
        private readonly IMapper _mapper;

        public DeletePlaylistCommandHandler(
            IPlaylistRepository playlistRepository,
            IViewerRepository viewerRepository,
            ILogger<DeletePlaylistCommandHandler> logger,
            IMapper mapper)
        {
            _playlistRepository = playlistRepository;
            _viewerRepository = viewerRepository;
            _logger = logger;
            _mapper = mapper;
        }
        
        public async Task<IList<PlaylistDto>> Handle(DeletePlaylistCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var playlist = await _playlistRepository.GetAsync(request.PlaylistId, cancellationToken)
                               ?? throw new ArgumentException("Playlist could not be found");

                var viewer = await _viewerRepository.GetAsync(request.MemberId, cancellationToken)
                             ?? throw new ArgumentException("Viewer could not be found");

                if (!playlist.Owner.Equals(viewer))
                {
                    throw new InformativeException("Playlist does not belong to this user");
                }

                await _playlistRepository.DeleteAsync(playlist.Id, cancellationToken);

                var remainingPlaylists =
                    await _playlistRepository.GetAllByViewerAsync(request.MemberId, cancellationToken);
                return _mapper.Map<IList<PlaylistDto>>(remainingPlaylists);
            }
            catch (InformativeException exception)
            {
                _logger.LogError(exception, $"Could not delete playlist {request.PlaylistId.ToString()} by member " +
                                            $"{request.MemberId.ToString()}");
                throw;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, $"Could not delete playlist {request.PlaylistId.ToString()} by member " +
                                 $"{request.MemberId.ToString()}");
                throw new InformativeException("Could not delete playlist. Please retry");
            }
        }
    }
}