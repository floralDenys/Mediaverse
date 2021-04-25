using System.Collections.Generic;

namespace Mediaverse.Application.JointContentConsumption.Common.Dtos
{
    public class PlaylistDto
    {
        public string Name { get; set; }
        public IList<PlaylistItemDto> Items { get; set; }
    }
}