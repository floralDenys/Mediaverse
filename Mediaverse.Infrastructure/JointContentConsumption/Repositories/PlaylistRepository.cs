using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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
        
        public PlaylistRepository(
            ApplicationDbContext applicationDbContext,
            IUserRepository userRepository)
        {
            _applicationDbContext = applicationDbContext;

            _userRepository = userRepository;
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
            var playlistDto = new PlaylistDto
            {
                Id = playlist.Id,
                OwnerId = playlist.Owner.Profile.Id,
                IsTemporary = playlist.IsTemporary,
                CurrentlyPlayingContentIndex = playlist.GetEnumerator()?.Current.PlaylistItemIndex ?? -1,
                ContentIds = playlist.ToList()
            };

            _applicationDbContext.Playlists.Add(playlistDto);

            return _applicationDbContext.SaveChangesAsync(cancellationToken);
        }

        public Task UpdateAsync(Playlist playlist, CancellationToken cancellationToken)
        {
            var playlistDto = _applicationDbContext.Playlists.Find(playlist.Id);
            playlistDto.IsTemporary = playlist.IsTemporary;
            playlistDto.CurrentlyPlayingContentIndex = playlist.GetEnumerator()?.Current.PlaylistItemIndex ?? -1;
            playlistDto.ContentIds = playlist.ToList();

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
                        new UserProfile(owner.Id, owner.Nickname, owner.Type == UserType.Member)));
    }
}