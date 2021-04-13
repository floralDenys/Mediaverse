using System.Collections.Generic;
using Mediaverse.Domain.ContentSearch.Entities;

namespace Mediaverse.Domain.ContentSearch.ValueObjects
{
    public class SearchResult
    {
        public IList<Preview> MatchingContentPreviews { get; }
        
        public Content RequestedContent { get; }

        public SearchResult(IList<Preview> matchingContentPreviews)
        {
            MatchingContentPreviews = matchingContentPreviews;
        }
        
        public SearchResult(Content requestedContent)
        {
            RequestedContent = requestedContent;
        }
    }
}