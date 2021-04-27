using System;
using System.Collections.Generic;

namespace Mediaverse.Application.JointContentConsumption.Common.Dtos
{
    public class PlaylistDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public IList<PlaylistItemDto> Items { get; set; }
    }
}