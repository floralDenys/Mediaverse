using System.Collections.Generic;
using Mediaverse.Domain.ContentSearch.Entities;

namespace Mediaverse.Domain.ContentSearch.ValueObjects
{
    public class SearchResult
    {
        public IList<Preview> PreviewList { get; }
    }
}