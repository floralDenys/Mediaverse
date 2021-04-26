using Mediaverse.Domain.ContentSearch.Enums;

namespace Mediaverse.Infrastructure.ContentSearch.Repositories.Dtos
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
        
        public string ThumbnailUrl { get; set; }
        public long ThumbnailWidth { get; set; }
        public long ThumbnailHeight { get; set; }
    }
}