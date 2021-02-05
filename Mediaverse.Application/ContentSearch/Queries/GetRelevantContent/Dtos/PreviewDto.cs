namespace Mediaverse.Application.ContentSearch.Queries.GetRelevantContent.Dtos
{
    public class PreviewDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public ExternalIdDto ExternalId { get; set; }
        public ThumbnailDto Thumbnail { get; set; }
    }
}