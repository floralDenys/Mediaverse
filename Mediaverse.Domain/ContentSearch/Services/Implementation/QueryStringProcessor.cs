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
    }
}