using Mediaverse.Application.ContentSearch.Queries.GetRelevantContent.Dtos;

namespace Mediaverse.Application.JointContentConsumption.Common.Dtos
{
    public class PlaylistItemDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public ThumbnailDto Thumbnail { get; set; }
    }
}