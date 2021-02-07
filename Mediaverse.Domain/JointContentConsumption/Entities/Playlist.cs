using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using Mediaverse.Domain.Common;
using Mediaverse.Domain.JointContentConsumption.ValueObjects;

namespace Mediaverse.Domain.JointContentConsumption.Entities
{
    public class Playlist : Entity, IEnumerable<Content>
    {
        private readonly IList<Content> _contents;
        
        public Host Owner { get; }
        public bool IsTemporary => Owner == null;
        
        public int Count => _contents.Count;

        private int? _currentlyPlayingContentIndex;
        
        public Playlist(Guid id, IEnumerable<Content> contents = null, Host owner = null) : base(id)
        {
            try
            {
                _contents = contents?.ToList() ?? new List<Content>();
                Owner = owner ?? throw new ArgumentNullException(nameof(owner));

                if (_contents.Any())
                {
                    _currentlyPlayingContentIndex = 0;
                }
            }
            catch (Exception exception)
            {
                throw new InvalidOperationException("Could not create playlist", exception);
            }
        }

        public IEnumerator<Content> GetEnumerator() => _contents.GetEnumerator();
        
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public void Add(Content content)
        {
            try
            {
                _ = content ?? throw new ArgumentNullException(nameof(content));

                if (Contains(content))
                {
                    throw new InvalidOperationException("Content is added already");
                }
                
                _contents.Add(content);

                if (_contents.Count == 1)
                {
                    _currentlyPlayingContentIndex = 0;
                }
            }
            catch (Exception exception)
            {
                throw new InvalidOperationException($"Could not add content {content} to playlist {this}", exception);
            }
        }
        
        public void Remove(Content content)
        {
            try
            {
                _ = content ?? throw new ArgumentNullException(nameof(content));

                if (!Contains(content))
                {
                    throw new InvalidOperationException("Playlist does not contain specified item");
                }
                
                if (!_contents.Remove(content))
                {
                    throw new InvalidOperationException("Something went wrong");
                }

                if (!_contents.Any())
                {
                    _currentlyPlayingContentIndex = null;
                }
            }
            catch (Exception exception)
            {
                throw new InvalidOperationException($"Could not remove content {content} from playlist {this}", exception);
            }
        }

        public Content PlayNextContent()
        {
            try
            {
                if (!_contents.Any())
                {
                    throw new InvalidOperationException("Playlist is empty");
                }

                if (_contents.Count == _currentlyPlayingContentIndex + 1)
                {
                    throw new InvalidOperationException("The end of the playlist is reached already");
                }

                ++_currentlyPlayingContentIndex;
                return _contents[_currentlyPlayingContentIndex.Value];
            }
            catch (Exception exception)
            {
                throw new InvalidOperationException($"Could not play next content from the playlist {this}", exception);
            }
        }

        public Content PlayPreviousContent()
        {
            try
            {
                if (!_contents.Any())
                {
                    throw new InvalidOperationException("Playlist is empty");
                }

                if (_currentlyPlayingContentIndex == 0)
                {
                    throw new InvalidOperationException("The start of the playlist is reached already");
                }

                --_currentlyPlayingContentIndex;
                return _contents[_currentlyPlayingContentIndex.Value];
            }
            catch (Exception exception)
            {
                throw new InvalidOperationException($"Could not play previous content from the playlist {this}", exception);
            }
        }
        
        public bool Contains(Content content) => _contents.Contains(content);
    }
}