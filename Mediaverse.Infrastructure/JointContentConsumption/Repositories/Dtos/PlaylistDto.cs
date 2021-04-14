using System;
using System.Collections.Generic;
using Mediaverse.Domain.JointContentConsumption.ValueObjects;

namespace Mediaverse.Infrastructure.JointContentConsumption.Repositories.Dtos
{
    public class PlaylistDto
    {
        public Guid Id { get; set; }
        public Guid OwnerId { get; set; }
        public bool IsTemporary { get; set; }
        public int? CurrentlyPlayingContentIndex { get; set; }
        public List<PlaylistItemDto> PlaylistItems { get; set; } 
    }
}