using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Mediaverse.Domain.Authentication.Entities;
using Mediaverse.Domain.Authentication.Enums;
using Mediaverse.Domain.Authentication.Repositories;
using Mediaverse.Domain.JointContentConsumption.Entities;
using Mediaverse.Domain.JointContentConsumption.Repositories;
using Mediaverse.Domain.JointContentConsumption.ValueObjects;
using Mediaverse.Infrastructure.Common.Persistence;
using Mediaverse.Infrastructure.JointContentConsumption.Repositories.Dtos;

namespace Mediaverse.Infrastructure.JointContentConsumption.Repositories
{
    public class PlaylistRepository : IPlaylistRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;

        private readonly IUserRepository _userRepository;

        private readonly IMapper _mapper;
        
        public PlaylistRepository(
            ApplicationDbContext applicationDbContext,
            IUserRepository userRepository, 
            IMapper mapper)
        {
            _applicationDbContext = applicationDbContext;
            _userRepository = userRepository;
            _mapper = mapper;
        }
        
        public async Task<Playlist> GetAsync(Guid playlistId, CancellationToken cancellationToken)
        {
            var playlistDto = _applicationDbContext.Playlists.Find(playlistId);
            var owner = await _userRepository.GetUserAsync(playlistDto.OwnerId, cancellationToken);

            return GetPlaylist(playlistDto, owner);
        }

        public async Task<IList<Playlist>> GetAllByViewerAsync(Guid ownerId, CancellationToken cancellationToken)
        {
            var owner = await _userRepository.GetUserAsync(ownerId, cancellationToken);
            var playlistDtos = _applicationDbContext.Playlists
                .Where(p => p.OwnerId == ownerId);

            return playlistDtos.Select(p => GetPlaylist(p, owner)).ToList();
        }

        public Task AddAsync(Playlist playlist, CancellationToken cancellationToken)
        {
            var playlistDto = _mapper.Map<PlaylistDto>(playlist);

            _applicationDbContext.Playlists.Add(playlistDto);

            return _applicationDbContext.SaveChangesAsync(cancellationToken);
        }

        public Task UpdateAsync(Playlist playlist, CancellationToken cancellationToken)
        {
            var playlistDto = _applicationDbContext.Playlists.Find(playlist.Id);
            playlistDto.IsTemporary = playlist.IsTemporary;
            playlistDto.CurrentlyPlayingContentIndex = playlist.GetEnumerator()?.Current?.PlaylistItemIndex ?? -1;
            playlistDto.PlaylistItems = playlist
                .Select(pi => 
                    new PlaylistItemDto
                    {
                        ExternalId = pi.ContentId.ExternalId,
                        ContentSource = pi.ContentId.ContentSource,
                        ContentType = pi.ContentId.ContentType,
                        PlaylistItemIndex = pi.PlaylistItemIndex
                    })
                .ToList();

            return _applicationDbContext.SaveChangesAsync(cancellationToken);
        }

        public Task DeleteAsync(Guid playlistId, CancellationToken cancellationToken)
        {
            var playlistDto = _applicationDbContext.Playlists.Find(playlistId);
            _applicationDbContext.Playlists.Remove(playlistDto);

            return _applicationDbContext.SaveChangesAsync(cancellationToken);
        }

        private Playlist GetPlaylist(PlaylistDto playlistDto, User owner) => 
            new Playlist(
                playlistDto.Id,
            new Viewer(
                        new UserProfile(owner.Id, owner.Nickname, owner.Type == UserType.Member)),
                playlistDto.PlaylistItems.Select(pi => 
                    new PlaylistItem(
                        new ContentId(pi.ExternalId, pi.ContentSource, pi.ContentType), pi.PlaylistItemIndex)));
    }
}