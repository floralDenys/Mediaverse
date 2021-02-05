using System.Collections.Generic;

namespace Mediaverse.Domain.ContentSearch.ValueObjects
{
    public class SearchResult
    {
        public IList<Preview> PreviewList { get; }
    }
}