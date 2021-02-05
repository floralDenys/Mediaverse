using System.Collections.Generic;
using MediatR;
using Mediaverse.Application.ContentSearch.Queries.GetRelevantContent.Dtos;
using Mediaverse.Domain.ContentSearch.Enums;

namespace Mediaverse.Application.ContentSearch.Queries.GetRelevantContent
{
    public class GetRelevantContentQuery : IRequest<SearchResultDto>
    {
        public MediaContentSource SelectedSource { get; set; }
        public string QueryString { get; set; }

        public override string ToString() => $"{QueryString} from {SelectedSource.ToString()}";
    }
}