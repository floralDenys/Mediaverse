using System;
using System.Collections.Generic;
using Mediaverse.Domain.Common;
using Mediaverse.Domain.JointContentConsumption.ValueObjects;

namespace Mediaverse.Domain.JointContentConsumption.Entities
{
    public class Room : Entity
    {
        private string _name;

        private Host _host;
        
        private IList<Viewer> _viewers;
        private int _maxViewersQuantity;

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

        public Host Host
        {
            get => _host;
            set => _host = value ?? throw new ArgumentException("Given host is null");
        }
        public Guid ActivePlaylistId { get; private set; }
        
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

        public Room(Guid id, string name, Host host) : base(id)
        {
            try
            {
                Name = name;
                Host = host;
                
                _maxViewersQuantity = int.MaxValue;
                _viewers = new List<Viewer>();
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
                
                if (!playlist.IsTemporary)
                {
                    if (playlist.Owner.Equals(_host))
                    {
                        throw new InvalidOperationException($"Playlist {playlist} does not belong to host {_host}");
                    }
                }

                ActivePlaylistId = playlist.Id;
            }
            catch (Exception exception)
            {
                throw new InvalidOperationException($"Could not update playlist {playlist} in room {this}", exception);
            }
        }
    }
}