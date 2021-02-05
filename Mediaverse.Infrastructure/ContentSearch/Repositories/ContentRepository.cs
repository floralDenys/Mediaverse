using System;
using System.Threading.Tasks;
using Mediaverse.Domain.ContentSearch.Enums;
using Mediaverse.Domain.ContentSearch.Repositories;
using Mediaverse.Domain.ContentSearch.ValueObjects;
using YouTubeRepository = Mediaverse.Infrastructure.ContentSearch.Repositories.YouTube.YouTubeRepository;

namespace Mediaverse.Infrastructure.ContentSearch.Repositories
{
    public class ContentRepository : IContentRepository
    {
        private readonly YouTubeRepository _youTubeRepository;

        public ContentRepository(
            YouTubeRepository youTubeRepository)
        {
            _youTubeRepository = youTubeRepository;
        }
        
        public Task<SearchResult> SearchForContent(
            MediaContentSource source,
            ContentQueryType contentQueryType,
            string queryString)
        {
            try
            {
                Task<SearchResult> searchResult;
                
                switch (source)
                {
                    case MediaContentSource.YouTube:
                        searchResult = contentQueryType == ContentQueryType.Keywords 
                            ? _youTubeRepository.SearchForVideosByKeywords(queryString) 
                            : _youTubeRepository.SearchForVideoById(queryString);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(MediaContentSource));
                }

                return searchResult;
            }
            catch (Exception exception)
            {
                throw new InvalidOperationException($"An exception occured on attempt to search the content for " +
                                                    $"{source} by {contentQueryType} (query string: {queryString})", exception);
            }
        }
    }
}