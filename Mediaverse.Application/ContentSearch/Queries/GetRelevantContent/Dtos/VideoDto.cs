namespace Mediaverse.Application.ContentSearch.Queries.GetRelevantContent.Dtos
{
    public class VideoDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public ExternalIdDto ExternalId { get; set; }
        public PlayerDto PlayerDto { get; set; }
    }
}