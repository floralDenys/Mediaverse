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
using Microsoft.EntityFrameworkCore;

namespace Mediaverse.Infrastructure.JointContentConsumption.Repositories
{
    public class PlaylistRepository : IPlaylistRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;

        private readonly IUserRepository _userRepository;
        private readonly IContentRepository _contentRepository;

        private readonly IMapper _mapper;
        
        public PlaylistRepository(
            ApplicationDbContext applicationDbContext,
            IUserRepository userRepository, 
            IContentRepository contentRepository,
            IMapper mapper)
        {
            _applicationDbContext = applicationDbContext;
            _userRepository = userRepository;
            _contentRepository = contentRepository;
            _mapper = mapper;
        }
        
        public async Task<Playlist> GetAsync(Guid playlistId, CancellationToken cancellationToken)
        {
            var playlistDto = _applicationDbContext.Playlists
                .Include(p => p.PlaylistItems)
                .First(p => p.Id == playlistId);
            var owner = await _userRepository.GetUserAsync(playlistDto.OwnerId, cancellationToken);

            return GetPlaylist(playlistDto, owner);
        }

        public async Task<IList<Playlist>> GetAllByViewerAsync(Guid ownerId, CancellationToken cancellationToken)
        {
            var owner = await _userRepository.GetUserAsync(ownerId, cancellationToken);
            var playlistDtos = _applicationDbContext.Playlists
                .Where(p => p.OwnerId == ownerId).ToList();

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
            playlistDto.PlaylistItems.AddRange(playlist
                .Where(pi => !playlistDto.PlaylistItems
                    .Any(pid => 
                        pid.ExternalId == pi.ContentId.ExternalId 
                        && pid.ContentSource == pi.ContentId.ContentSource 
                        && pid.ContentType == pi.ContentId.ContentType))
                .Select(pi => 
                    new PlaylistItemDto
                    {
                        ExternalId = pi.ContentId.ExternalId,
                        ContentSource = pi.ContentId.ContentSource,
                        ContentType = pi.ContentId.ContentType,
                        PlaylistItemIndex = pi.PlaylistItemIndex,
                        PlaylistId = playlistDto.Id
                    }));
            
            playlistDto.PlaylistItems.RemoveAll(pid => 
                !playlist.Any(pi =>
                    pi.ContentId.ExternalId == pid.ExternalId
                    && pi.ContentId.ContentSource == pid.ContentSource
                    && pi.ContentId.ContentType == pid.ContentType));

            return _applicationDbContext.SaveChangesAsync(cancellationToken);
        }

        public Task DeleteAsync(Guid playlistId, CancellationToken cancellationToken)
        {
            var playlistDto = _applicationDbContext.Playlists.Find(playlistId);
            _applicationDbContext.Playlists.Remove(playlistDto);

            return _applicationDbContext.SaveChangesAsync(cancellationToken);
        }

        private Playlist GetPlaylist(PlaylistDto playlistDto, User owner)
        {
            var playlistItems = playlistDto.PlaylistItems?
                .Select(async pi => await GetPlaylistItem(pi))
                .Select(t => t.Result)
                .Where(t => t != null)
                .ToList();
            
            return new Playlist(
                playlistDto.Id,
                playlistDto.Name,
                new Viewer(
                    new UserProfile(owner.Id, owner.UserName, owner.Type == UserType.Member)),
                playlistItems);
        }

        private async Task<PlaylistItem> GetPlaylistItem(PlaylistItemDto playlistItemDto)
        {
            var contentId = new ContentId(
                playlistItemDto.ExternalId,
                playlistItemDto.ContentSource,
                playlistItemDto.ContentType);

            var content = await _contentRepository.GetAsync(contentId, CancellationToken.None);
            return new PlaylistItem(
                contentId,
                playlistItemDto.PlaylistItemIndex,
                content.Title,
                content.Description);
        }
    }
}