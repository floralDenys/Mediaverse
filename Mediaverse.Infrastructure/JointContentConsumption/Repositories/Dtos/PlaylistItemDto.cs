using Mediaverse.Domain.JointContentConsumption.Enums;

namespace Mediaverse.Infrastructure.JointContentConsumption.Repositories.Dtos
{
    public class PlaylistItemDto
    {
        public string ExternalId { get; set; }
        public MediaContentSource ContentSource { get; set; }
        public MediaContentType ContentType { get; set; }
        public int PlaylistItemIndex { get; set; }
    }
}