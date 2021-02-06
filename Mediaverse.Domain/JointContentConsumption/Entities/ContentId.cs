using Mediaverse.Domain.JointContentConsumption.Enums;

namespace Mediaverse.Domain.JointContentConsumption.Entities
{
    public class ContentId
    {
        public string ExternalId { get; }
        public MediaContentSource ContentSource { get; } 
    }
}