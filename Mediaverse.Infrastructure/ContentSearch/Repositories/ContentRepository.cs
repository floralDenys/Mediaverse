using System;
using System.Collections.Generic;
using Mediaverse.Domain.ContentSearch.Entities;
using Mediaverse.Domain.ContentSearch.Enums;
using Mediaverse.Domain.ContentSearch.Repositories;
using Mediaverse.Domain.ContentSearch.Services;
using Mediaverse.Infrastructure.YouTube;

namespace Mediaverse.Infrastructure.ContentSearch.Repositories
{
    public class ContentRepository : IContentRepository
    {
        private readonly IQueryStringProcessor _queryStringProcessor;
        private readonly YouTubeRepository _youTubeRepository;

        public ContentRepository(
            IQueryStringProcessor queryStringProcessor,
            YouTubeRepository youTubeRepository)
        {
            _queryStringProcessor = queryStringProcessor;
            _youTubeRepository = youTubeRepository;
        }
        
        public IList<Preview> SearchForContent(
            MediaContentSource source,
            ContentQueryType contentQueryType,
            string queryString)
        {
            try
            {
                switch (source)
                {
                    case MediaContentSource.YouTube:
                        if (contentQueryType == ContentQueryType.Keywords)
                        {
                            _youTubeRepository.SearchForVideosByKeywords(queryString);
                        }
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(MediaContentSource));
                }
                
                throw new NotImplementedException();
            }
            catch (Exception exception)
            {
                throw new NotImplementedException();
            }
        }
    }
}