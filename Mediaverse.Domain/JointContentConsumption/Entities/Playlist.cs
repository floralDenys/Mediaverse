using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using Mediaverse.Domain.Common;
using Mediaverse.Domain.JointContentConsumption.ValueObjects;

namespace Mediaverse.Domain.JointContentConsumption.Entities
{
    public class Playlist : Entity, IEnumerable<PlaylistItem>
    {
        private string _name;

        public string Name
        {
            get => _name;
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new InformativeException("Playlist name can not be empty");
                }

                _name = value;
            }
        }
        
        private readonly IList<PlaylistItem> _items;
        
        public Viewer Owner { get; }
        public bool IsTemporary { get; set; }
        
        public Playlist(
            Guid id,
            string name,
            Viewer owner,
            IEnumerable<PlaylistItem> items = null,
            int? currentlyPlayingContentIndex = null) : base(id)
        {
            try
            {
                Name = name;
                Owner = owner ?? throw new ArgumentNullException(nameof(owner));

                _items = items?.ToList() ?? new List<PlaylistItem>();
                if (_items.Any())
                {
                    // sort items by their playlist indexes 
                    _items = _items.OrderBy(x => x.PlaylistItemIndex).ToList();
                }
            }
            catch (InformativeException)
            {
                throw;
            }
            catch (Exception exception)
            {
                throw new InvalidOperationException("Could not create playlist", exception);
            }
        }

        public IEnumerator<PlaylistItem> GetEnumerator() => _items.GetEnumerator();
        
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public void Add(ContentId contentId)
        {
            _ = contentId ?? throw new ArgumentNullException(nameof(contentId));
            if (Contains(contentId))
            {
                throw new InformativeException("Content is added already");
            }

            int playlistItemIndex = (_items.LastOrDefault()?.PlaylistItemIndex ?? 0) + 1;
            _items.Add(new PlaylistItem(contentId, playlistItemIndex));
        }
        
        public void Remove(ContentId contentId)
        {
            _ = contentId ?? throw new ArgumentNullException(nameof(contentId));

            var playlistItem = _items.First(x => x.ContentId.Equals(contentId));
            if (playlistItem == null)
            {
                throw new InformativeException("Playlist does not contain specified item");
            }
            
            if (!_items.Remove(playlistItem))
            {
                throw new InvalidOperationException("Something went wrong");
            }
        }

        public ContentId GetNextContent(ContentId contentId)
        {
            if (!_items.Any())
            {
                throw new InformativeException("Playlist is empty");
            }

            PlaylistItem playlistItem = null;
            if (contentId != null)
            {
                playlistItem = _items.FirstOrDefault(i => i.ContentId.Equals(contentId)) 
                               ?? throw new InvalidOperationException(
                                   "Specified content does not belong to the playlist");
            }

            var nextPlaylistItem = _items
                .FirstOrDefault(i => 
                    i.PlaylistItemIndex > (playlistItem?.PlaylistItemIndex ?? 0));
            
            if (nextPlaylistItem == null)
            {
                throw new InformativeException("The end of the playlist is reached already");
            }

            return nextPlaylistItem.ContentId;
        }

        public ContentId GetPreviousContent(ContentId contentId)
        {
            if (!_items.Any())
            {
                throw new InformativeException("Playlist is empty");
            }

            PlaylistItem playlistItem = null;
            if (contentId != null)
            {
                playlistItem = _items.FirstOrDefault(i => i.ContentId.Equals(contentId))
                               ?? throw new InvalidOperationException(
                                   "Specified content does not belong to the playlist");
            }

            var previousPlaylistItem =
                _items.LastOrDefault(i =>
                    i.PlaylistItemIndex < (playlistItem?.PlaylistItemIndex ?? 0));

            if (previousPlaylistItem == null)
            {
                throw new InformativeException("The start of the playlist is reached already");
            }


            return previousPlaylistItem.ContentId;
        }

        public bool Contains(ContentId contentId) => _items
            .Select(x => x.ContentId)
            .Contains(contentId);
    }
}