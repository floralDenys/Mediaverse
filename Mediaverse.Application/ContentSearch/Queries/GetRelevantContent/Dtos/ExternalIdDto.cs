using Mediaverse.Domain.ContentSearch.Enums;

namespace Mediaverse.Application.ContentSearch.Queries.GetRelevantContent.Dtos
{
    public class ExternalIdDto
    {
        public string Id { get; set; }
        public MediaContentSource ContentSource { get; set; }
        public ContentType ContentType { get; set; }
    }
}