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
        private readonly IList<PlaylistItem> _items;
        
        public Viewer Owner { get; }
        public bool IsTemporary { get; set; }
        
        public int? CurrentlyPlayingContentIndex { get; private set; }
        
        public Playlist(Guid id, Viewer owner, IEnumerable<PlaylistItem> items = null) : base(id)
        {
            try
            {
                _items = items?.ToList() ?? new List<PlaylistItem>();

                Owner = owner ?? throw new ArgumentNullException(nameof(owner));

                if (_items.Any())
                {
                    CurrentlyPlayingContentIndex = 0;

                    // sort items by their playlist indexes 
                    _items = _items.OrderBy(x => x.PlaylistItemIndex).ToList();
                }
            }
            catch (Exception exception)
            {
                throw new InvalidOperationException("Could not create playlist", exception);
            }
        }
        
        private Playlist() { }

        public IEnumerator<PlaylistItem> GetEnumerator() => _items.GetEnumerator();
        
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public void Add(ContentId contentId)
        {
            try
            {
                _ = contentId ?? throw new ArgumentNullException(nameof(contentId));

                if (Contains(contentId))
                {
                    throw new InvalidOperationException("Content is added already");
                }

                int playlistItemIndex = (_items.LastOrDefault()?.PlaylistItemIndex ?? 0) + 1;
                _items.Add(new PlaylistItem(contentId, playlistItemIndex));

                if (_items.Count == 1)
                {
                    CurrentlyPlayingContentIndex = 1;
                }
            }
            catch (Exception exception)
            {
                throw new InvalidOperationException($"Could not add content {contentId} to playlist {this}", exception);
            }
        }
        
        public void Remove(ContentId contentId)
        {
            try
            {
                _ = contentId ?? throw new ArgumentNullException(nameof(contentId));

                var playlistItem = _items.First(x => x.ContentId.Equals(contentId));
                if (playlistItem == null)
                {
                    throw new InvalidOperationException("Playlist does not contain specified item");
                }
                
                if (!_items.Remove(playlistItem))
                {
                    throw new InvalidOperationException("Something went wrong");
                }

                if (!_items.Any())
                {
                    CurrentlyPlayingContentIndex = null;
                }
            }
            catch (Exception exception)
            {
                throw new InvalidOperationException($"Could not remove content {contentId} from playlist {this}", exception);
            }
        }

        public ContentId PlayNextContent()
        {
            try
            {
                if (!_items.Any())
                {
                    throw new InvalidOperationException("Playlist is empty");
                }

                var nextPlaylistItem = _items
                    .FirstOrDefault(i => i.PlaylistItemIndex > CurrentlyPlayingContentIndex);
                
                if (nextPlaylistItem == null)
                {
                    throw new InvalidOperationException("The end of the playlist is reached already");
                }

                CurrentlyPlayingContentIndex = nextPlaylistItem.PlaylistItemIndex;
                
                return nextPlaylistItem.ContentId;
            }
            catch (Exception exception)
            {
                throw new InvalidOperationException($"Could not play next content from the playlist {this}", exception);
            }
        }

        public ContentId PlayPreviousContent()
        {
            try
            {
                if (!_items.Any())
                {
                    throw new InvalidOperationException("Playlist is empty");
                }

                var previousPlaylistItem =
                    _items.LastOrDefault(i => i.PlaylistItemIndex < CurrentlyPlayingContentIndex); 
                
                if (previousPlaylistItem == null)
                {
                    throw new InvalidOperationException("The start of the playlist is reached already");
                }

                CurrentlyPlayingContentIndex = previousPlaylistItem.PlaylistItemIndex;
                
                return previousPlaylistItem.ContentId;
            }
            catch (Exception exception)
            {
                throw new InvalidOperationException($"Could not play previous content from the playlist {this}", exception);
            }
        }
        
        private bool Contains(ContentId contentId) => _items
            .Select(x => x.ContentId)
            .Contains(contentId);
    }
}