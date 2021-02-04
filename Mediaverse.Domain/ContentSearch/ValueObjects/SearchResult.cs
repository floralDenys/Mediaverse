using System.Collections.Generic;
using Mediaverse.Domain.ContentSearch.Entities;

namespace Mediaverse.Domain.ContentSearch.ValueObjects
{
    public class SearchResult
    {
        public Preview Preview { get; }
        public IList<Preview> PreviewList { get; }

        public SearchResult(
            Preview preview = null,
            IList<Preview> previewList = null)
        {
            Preview = preview;
            PreviewList = previewList;
        }
    }
}