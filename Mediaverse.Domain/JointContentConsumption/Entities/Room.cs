using System;
using System.Collections.Generic;
using Mediaverse.Domain.Common;
using Mediaverse.Domain.JointContentConsumption.Enums;
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
        public string Description { get; set; }

        public RoomType Type { get; private set; } = RoomType.Public;
        private Invitation _invitation;

        public Invitation Invitation
        {
            get => _invitation;
            set => _invitation = value 
                                 ?? throw new InvalidOperationException("Room requires invitation");
        }
        
        public Viewer Host { get; private set; }

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

        public Room(
            Guid id,
            string name,
            Viewer host,
            RoomType type,
            Invitation invitation,
            string description = "") : base(id)
        {
            try
            {
                Name = name;
                Host = host;
                Type = type;
                Invitation = invitation;
            }
            catch (Exception exception)
            {
                throw new InvalidOperationException("Could not create room", exception);
            }
        }

        public Room(
            Guid id,
            string name,
            string description,
            Viewer host,
            RoomType type,
            Invitation invitation,
            int maxViewersQuantity,
            Guid activePlaylistId,
            IList<Viewer> viewers) : base(id)
        {
            Name = name;
            Description = description;
            Host = host;
            Type = type;
            Invitation = invitation;
            MaxViewersQuantity = maxViewersQuantity;
            ActivePlaylistId = activePlaylistId;
            _viewers = viewers;
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
                if (!IsSpotAvailable)
                {
                    throw new InvalidOperationException("There is no spot for the viewer");
                }
                
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

                bool isHostLeavingTheRoom = Host.Equals(viewer);
                
                if (isHostLeavingTheRoom)
                {
                    SelectNewHost();
                }
                else
                {
                    _viewers.Remove(viewer);
                }
            }
            catch (Exception exception)
            {
                throw new InvalidOperationException($"Viewer {viewer?.Profile.Id.ToString()} could not leave " +
                                                    $"the room {Id.ToString()}", exception);
            }
        }
        
        private bool IsSpotAvailable => _viewers.Count < _maxViewersQuantity;

        private void SelectNewHost()
        {
            var random = new Random();
            
            int newHostIndex = random.Next(0, _viewers.Count - 1);
            Host = _viewers[newHostIndex];
            
            _viewers.Remove(Host);
        }
    }
}