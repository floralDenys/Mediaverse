using System;
using System.Collections.Generic;
using Mediaverse.Application.JointContentConsumption.Queries.GetPlaylist.Dtos;

namespace Mediaverse.Application.JointContentConsumption.Common.Dtos
{
    public class PlaylistDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public IList<PlaylistItemDto> Items { get; set; }
        
        public IList<SelectablePlaylistDto> AvailablePlaylists { get; set; }
    }
}