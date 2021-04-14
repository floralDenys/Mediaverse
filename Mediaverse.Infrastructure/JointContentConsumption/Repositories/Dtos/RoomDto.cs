using System;
using System.Collections.Generic;
using Mediaverse.Domain.JointContentConsumption.Enums;

namespace Mediaverse.Infrastructure.JointContentConsumption.Repositories.Dtos
{
    public class RoomDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public RoomType Type { get; set; }
        public Guid HostId { get; set; }
        public Guid ActivePlaylistId { get; set; }
        public int MaxViewersQuantity { get; set; }
        public List<ViewerDto> Viewers { get; set; }
    }
}