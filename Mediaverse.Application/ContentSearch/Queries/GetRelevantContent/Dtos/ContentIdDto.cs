using Mediaverse.Domain.ContentSearch.Enums;

namespace Mediaverse.Application.ContentSearch.Queries.GetRelevantContent.Dtos
{
    public class ContentIdDto
    {
        public string Id { get; set; }
        public MediaContentSource ContentSource { get; set; }
        public MediaContentType MediaContentType { get; set; }
    }
}