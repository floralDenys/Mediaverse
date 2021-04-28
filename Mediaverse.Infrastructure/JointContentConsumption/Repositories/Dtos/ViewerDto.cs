using System;

namespace Mediaverse.Infrastructure.JointContentConsumption.Repositories.Dtos
{
    public class ViewerDto
    {
        public Guid Id { get; set; }
        public RoomDto Room { get; set; }
    }
}