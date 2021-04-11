using System;
using System.Collections.Generic;

namespace Mediaverse.Infrastructure.JointContentConsumption.Repositories.Dtos
{
    public class RoomDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid Host { get; set; }
        public Guid ActivePlaylistId { get; set; }
        public int MaxViewersQuantity { get; set; }
        public List<ViewerDto> Viewers { get; set; }
    }
}