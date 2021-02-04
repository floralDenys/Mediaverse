using System;
using System.Collections.Generic;
using Mediaverse.Domain.ContentSearch.Enums;

namespace Mediaverse.Domain.ContentSearch.Services.Implementation
{
    public class QueryStringProcessor : IQueryStringProcessor
    {
        private readonly IDictionary<MediaContentSource, string> MediaContentSourceDomains =
            new Dictionary<MediaContentSource, string>
            {
                { MediaContentSource.YouTube, "youtube.com/watch"}
            };

        public ContentQueryType DefineQueryStringType(MediaContentSource source, string queryString)
        {
            if (!MediaContentSourceDomains.Keys.Contains(source))
            {
                throw new InvalidOperationException("Request domain is not specified for selected content source");
            }

            string selectedSourceDomain = MediaContentSourceDomains[source];
            return queryString.Contains(selectedSourceDomain)
                ? ContentQueryType.Link
                : ContentQueryType.Keywords;
        }
    }
}