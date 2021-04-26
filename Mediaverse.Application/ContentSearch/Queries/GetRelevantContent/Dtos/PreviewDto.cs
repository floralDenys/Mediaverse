namespace Mediaverse.Application.ContentSearch.Queries.GetRelevantContent.Dtos
{
    public class PreviewDto
    {
        public ContentIdDto ContentId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public ThumbnailDto Thumbnail { get; set; }
    }
}