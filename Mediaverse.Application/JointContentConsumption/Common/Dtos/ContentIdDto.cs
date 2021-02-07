using Mediaverse.Domain.JointContentConsumption.Enums;

namespace Mediaverse.Application.JointContentConsumption.Common.Dtos
{
    public class ContentIdDto
    {
        public string ExternalId { get; set; }
        public MediaContentSource ContentSource { get; set; }
        public MediaContentType ContentType { get; set; }
    }
}