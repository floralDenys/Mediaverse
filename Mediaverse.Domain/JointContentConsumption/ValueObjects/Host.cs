using System;
using System.Collections.Generic;
using System.Linq;
using Mediaverse.Domain.JointContentConsumption.Entities;

namespace Mediaverse.Domain.JointContentConsumption.ValueObjects
{
    public class Host : IEquatable<Host>
    {
        private readonly IList<Guid> _playlistIds;
        public UserProfile Profile { get; }
        public IReadOnlyList<Guid> PlaylistIds => (IReadOnlyList<Guid>)_playlistIds;

        public Host(UserProfile profile, IEnumerable<Guid> playlistIds = null)
        {
            try
            {
                Profile = profile ?? throw new ArgumentNullException(nameof(profile));
                _playlistIds = playlistIds?.ToList() ?? new List<Guid>();
            }
            catch (Exception exception)
            {
                throw new InvalidOperationException("Could not create host", exception);
            }
        }
        
        public void AddPlaylist(Playlist playlist)
        {
            try
            {
                _ = playlist ?? throw new ArgumentNullException(nameof(playlist));
                
                if (_playlistIds.Contains(playlist.Id))
                {
                    throw new InvalidOperationException("Playlist is added already");
                }
                
                _playlistIds.Add(playlist.Id);
            }
            catch (Exception exception)
            {
                throw new InvalidOperationException($"Could not add playlist {playlist} to {this}", exception);
            }
        }

        public void RemovePlaylist(Playlist playlist)
        {
            try
            {
                _ = playlist ?? throw new ArgumentNullException(nameof(playlist));
                
                if (!_playlistIds.Contains(playlist.Id))
                {
                    throw new InvalidOperationException($"Playlist was not added");
                }

                _playlistIds.Remove(playlist.Id);
            }
            catch (Exception exception)
            {
                throw new InvalidOperationException($"Could not remove playlist {playlist} from {this}", exception);
            }
        }

        public override string ToString() => $"{Profile}";

        public bool Equals(Host other)
        {
            if (other == null)
            {
                return false;
            }
            
            return Equals(Profile, other.Profile);
        }

        public override bool Equals(object obj)
        {
            if (obj is Host other)
            {
                return Equals(other);
            }

            return false;
        }

        public override int GetHashCode()
        {
            return Profile.GetHashCode();
        }
    }
}