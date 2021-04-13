using Mediaverse.Domain.JointContentConsumption.Enums;

namespace Mediaverse.Infrastructure.JointContentConsumption.Repositories.Dtos
{
    public class ContentDto
    {
        public string ExternalId { get; set; }
        public MediaContentSource ContentSource { get; set; }
        public MediaContentType ContentType { get; set; }
        
        public string Title { get; set; }
        public string Description { get; set; }
        
        public string ContentPlayerHtml { get; set; }
        public int ContentPlayerWidth { get; set; }
        public int ContentPlayerHeight { get; set; }
    }
}