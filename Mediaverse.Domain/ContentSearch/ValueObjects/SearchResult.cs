using System.Collections.Generic;
using Mediaverse.Domain.JointContentConsumption.Entities;

namespace Mediaverse.Domain.ContentSearch.ValueObjects
{
    public class SearchResult
    {
        public Content SpecificContent { get; }
        public IList<Content> ContentList { get; }

        public SearchResult(
            Content specificContent = null,
            IList<Content> contentList = null)
        {
            SpecificContent = specificContent;
            ContentList = contentList;
        }
    }
}