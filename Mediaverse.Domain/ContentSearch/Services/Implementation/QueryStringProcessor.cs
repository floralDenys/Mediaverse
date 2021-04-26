using System;
using System.Collections.Generic;
using Mediaverse.Domain.ContentSearch.Enums;

namespace Mediaverse.Domain.ContentSearch.Services.Implementation
{
    public class QueryStringProcessor : IQueryStringProcessor
    {
        private readonly IDictionary<MediaContentSource, string> _mediaContentSourceDomains =
            new Dictionary<MediaContentSource, string>
            {
                { MediaContentSource.YouTube, "youtube.com/watch"}
            };
        
        private readonly  IDictionary<MediaContentSource, Tuple<string, string>> _contentIdScope =
            new Dictionary<MediaContentSource, Tuple<string, string>>
            {
                { MediaContentSource.YouTube, new Tuple<string, string>("v=", "&") }
            };

        public ContentQueryType DefineQueryStringType(MediaContentSource source, string queryString)
        {
            if (!_mediaContentSourceDomains.Keys.Contains(source))
            {
                throw new InvalidOperationException("Request domain is not specified for selected content source");
            }

            string selectedSourceDomain = _mediaContentSourceDomains[source];
            return queryString.Contains(selectedSourceDomain)
                ? ContentQueryType.ContentId
                : ContentQueryType.Keywords;
        }

        public string ExtractExternalContentIdFromUrl(MediaContentSource source, string queryString)
        {
            var selectedSourceScope = _contentIdScope[source];
            
            int beginningPosition = queryString.IndexOf(selectedSourceScope.Item1, StringComparison.Ordinal) 
                + selectedSourceScope.Item1.Length;
            int endingPosition = queryString.IndexOf(selectedSourceScope.Item2, StringComparison.Ordinal);
            endingPosition = endingPosition == -1 ? queryString.Length - 1 : endingPosition;
            
            return queryString.Substring(beginningPosition, endingPosition - beginningPosition + 1);
        }
    }
}