using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using Mediaverse.Domain.Common;
using Mediaverse.Domain.JointContentConsumption.ValueObjects;

namespace Mediaverse.Domain.JointContentConsumption.Entities
{
    public class Playlist : Entity, IEnumerable<ContentId>
    {
        private readonly IList<ContentId> _contentIds;
        
        public Viewer Owner { get; }
        public bool IsTemporary { get; set; }
        
        private int? _currentlyPlayingContentIndex;
        
        public Playlist(Guid id, Viewer owner, IEnumerable<ContentId> contentIds = null) : base(id)
        {
            try
            {
                _contentIds = contentIds?.ToList() ?? new List<ContentId>();
                Owner = owner ?? throw new ArgumentNullException(nameof(owner));

                if (_contentIds.Any())
                {
                    _currentlyPlayingContentIndex = 0;
                }
            }
            catch (Exception exception)
            {
                throw new InvalidOperationException("Could not create playlist", exception);
            }
        }
        
        private Playlist() { }

        public IEnumerator<ContentId> GetEnumerator() => _contentIds.GetEnumerator();
        
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
                
                _contentIds.Add(contentId);

                if (_contentIds.Count == 1)
                {
                    _currentlyPlayingContentIndex = 0;
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

                if (!Contains(contentId))
                {
                    throw new InvalidOperationException("Playlist does not contain specified item");
                }
                
                if (!_contentIds.Remove(contentId))
                {
                    throw new InvalidOperationException("Something went wrong");
                }

                if (!_contentIds.Any())
                {
                    _currentlyPlayingContentIndex = null;
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
                if (!_contentIds.Any())
                {
                    throw new InvalidOperationException("Playlist is empty");
                }

                if (_contentIds.Count == _currentlyPlayingContentIndex + 1)
                {
                    throw new InvalidOperationException("The end of the playlist is reached already");
                }

                ++_currentlyPlayingContentIndex;
                return _contentIds[_currentlyPlayingContentIndex.Value];
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
                if (!_contentIds.Any())
                {
                    throw new InvalidOperationException("Playlist is empty");
                }

                if (_currentlyPlayingContentIndex == 0)
                {
                    throw new InvalidOperationException("The start of the playlist is reached already");
                }

                --_currentlyPlayingContentIndex;
                return _contentIds[_currentlyPlayingContentIndex.Value];
            }
            catch (Exception exception)
            {
                throw new InvalidOperationException($"Could not play previous content from the playlist {this}", exception);
            }
        }
        
        private bool Contains(ContentId contentId) => _contentIds.Contains(contentId);
    }
}