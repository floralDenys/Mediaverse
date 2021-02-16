using System;
using System.Collections.Generic;
using Mediaverse.Domain.Common;
using Mediaverse.Domain.JointContentConsumption.ValueObjects;

namespace Mediaverse.Domain.JointContentConsumption.Entities
{
    public class Room : Entity
    {
        private string _name;
        public string Name
        {
            get => _name;
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentException("Given name is invalid");
                }

                _name = value;
            }
        }
        
        private int _hostViewerIndex = 0;
        public Viewer Host => Viewers[_hostViewerIndex];
        
        private IList<Viewer> _viewers;
        private int _maxViewersQuantity = int.MaxValue;
        
        public Guid ActivePlaylistId { get; private set; }
        public bool IsPlaylistSelected => ActivePlaylistId != default;
        
        public IReadOnlyList<Viewer> Viewers => (IReadOnlyList<Viewer>)_viewers;
        public int MaxViewersQuantity
        {
            get => _maxViewersQuantity;
            set
            {
                if (value < 1)
                {
                    throw new ArgumentException("Max viewers quantity should be more than 1");
                }

                _maxViewersQuantity = value;
            }
        }

        public Room(Guid id, string name, Viewer host) : base(id)
        {
            try
            {
                if (string.IsNullOrEmpty(name))
                {
                    throw new ArgumentException("Name could not be null or empty");
                }
                
                Name = name;
                _viewers = new List<Viewer>() { host };
            }
            catch (Exception exception)
            {
                throw new InvalidOperationException("Could not create room", exception);
            }
        }

        public void UpdateSelectedPlaylist(Playlist playlist)
        {
            try
            {
                _ = playlist ?? throw new ArgumentNullException(nameof(playlist));
                
                if (!playlist.Owner.Equals(Host))
                {
                    throw new InvalidOperationException($"Playlist {playlist} does not belong to host {Host}");
                }

                ActivePlaylistId = playlist.Id;
            }
            catch (Exception exception)
            {
                throw new InvalidOperationException($"Could not update playlist {playlist} in room {this}", exception);
            }
        }

        public void Join(Viewer viewer)
        {
            try
            {
                _ = viewer ?? throw new ArgumentNullException(nameof(viewer));

                if (_viewers.Contains(viewer))
                {
                    throw new InvalidOperationException("Viewer joined the room already");
                }
                
                _viewers.Add(viewer);
            }
            catch (Exception exception)
            {
                throw new InvalidOperationException($"Viewer {viewer?.Profile.Id.ToString()} could not join " +
                                                    $"the room {Id.ToString()}", exception);
            }
        }

        public void Leave(Viewer viewer)
        {
            try
            {
                _ = viewer ?? throw new ArgumentNullException(nameof(viewer));

                if (!_viewers.Contains(viewer))
                {
                    throw new InvalidOperationException("Viewer is not in the room");
                }

                _viewers.Remove(viewer);
            }
            catch (Exception exception)
            {
                throw new InvalidOperationException($"Viewer {viewer?.Profile.Id.ToString()} could not leave " +
                                                    $"the room {Id.ToString()}", exception);
            }
        }
        
        public bool IsSpotAvailable => _viewers.Count < _maxViewersQuantity;
    }
}