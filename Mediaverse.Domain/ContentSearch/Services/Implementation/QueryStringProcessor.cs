using System;
using System.Collections.Generic;
using Mediaverse.Domain.ContentSearch.Enums;
using Mediaverse.Domain.ContentSearch.Services;

namespace Mediaverse.Infrastructure.Common.Services.Implementation
{
    public class QueryStringProcessor : IQueryStringProcessor
    {
        private readonly IDictionary<MediaContentSource, string> MediaContentSourceDomains =
            new Dictionary<MediaContentSource, string>
            {
                { MediaContentSource.YouTube, "youtube.com/watch"}
            };

        public QueryStringType DefineQueryStringType(MediaContentSource source, string queryString)
        {
            if (!MediaContentSourceDomains.Keys.Contains(source))
            {
                throw new InvalidOperationException("Request domain is not specified for selected content source");
            }

            string selectedSourceDomain = MediaContentSourceDomains[source];
            return queryString.Contains(selectedSourceDomain)
                ? QueryStringType.Link
                : QueryStringType.Keywords;
        }
    }
}