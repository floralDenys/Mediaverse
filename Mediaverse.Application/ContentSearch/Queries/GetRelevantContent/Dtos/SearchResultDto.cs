using System.Collections.Generic;

namespace Mediaverse.Application.ContentSearch.Queries.GetRelevantContent.Dtos
{
    public class SearchResultDto
    {
        public IList<PreviewDto> Videos { get; set; }
    }
}