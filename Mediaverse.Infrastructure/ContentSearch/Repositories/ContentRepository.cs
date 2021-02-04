using System;
using Mediaverse.Domain.ContentSearch.Enums;
using Mediaverse.Domain.ContentSearch.Repositories;
using Mediaverse.Domain.ContentSearch.Services;
using Mediaverse.Domain.ContentSearch.ValueObjects;
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
        
        public SearchResult SearchForContent(MediaContentSource source, string queryString)
        {
            try
            {
                var queryStringType = _queryStringProcessor.DefineQueryStringType(source, queryString);
                
                switch (source)
                {
                    case MediaContentSource.YouTube:
                        if (queryStringType == QueryStringType.Keywords)
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