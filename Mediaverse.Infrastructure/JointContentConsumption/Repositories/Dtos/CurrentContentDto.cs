using System;
using Mediaverse.Domain.JointContentConsumption.Enums;

namespace Mediaverse.Infrastructure.JointContentConsumption.Repositories.Dtos
{
    public class CurrentContentDto
    {
        public RoomDto Room { get; set; }
        public Guid RoomId { get; set; }
        
        public string ExternalId { get; set; }
        public MediaContentSource Source { get; set; }
        public MediaContentType Type { get; set; }
        
        public ContentPlayerState PlayingState { get; set; }
        public long PlayingTime { get; set; }
        public DateTime LastUpdatedPlayingTime { get; set; }
    }
}