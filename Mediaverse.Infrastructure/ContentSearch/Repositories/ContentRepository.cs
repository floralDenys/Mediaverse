using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Mediaverse.Domain.ContentSearch.Enums;
using Mediaverse.Domain.ContentSearch.Repositories;
using Mediaverse.Domain.ContentSearch.ValueObjects;
using Mediaverse.Infrastructure.Common.Persistence;
using Mediaverse.Infrastructure.ContentSearch.Repositories.Dtos;
using Microsoft.EntityFrameworkCore;
using YouTubeRepository = Mediaverse.Infrastructure.ContentSearch.Repositories.YouTube.YouTubeRepository;

namespace Mediaverse.Infrastructure.ContentSearch.Repositories
{
    public class ContentRepository : IContentRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;
        
        private readonly YouTubeRepository _youTubeRepository;

        private readonly IMapper _mapper;

        public ContentRepository(
            ApplicationDbContext applicationDbContext,
            YouTubeRepository youTubeRepository,
            IMapper mapper)
        {
            _applicationDbContext = applicationDbContext;
            _youTubeRepository = youTubeRepository;
            _mapper = mapper;
        }
        
        public async Task<SearchResult> SearchForContentAsync(
            MediaContentSource source,
            ContentQueryType contentQueryType,
            string queryString,
            CancellationToken cancellationToken)
        {
            try
            {
                SearchResult searchResult;

                switch (source)
                {
                    case MediaContentSource.YouTube:
                        if (contentQueryType == ContentQueryType.Keywords)
                        {
                            searchResult = await _youTubeRepository.SearchForVideosByKeywords(queryString, cancellationToken);
                        }
                        else
                        {
                            var cachedContent = await _applicationDbContext.CachedContent.FirstOrDefaultAsync(
                                cc => cc.ExternalId == queryString && cc.ContentSource == source,
                                cancellationToken);

                            if (cachedContent != null)
                            {
                                searchResult = new SearchResult {RequestedContent = _mapper.Map<Content>(cachedContent)};
                            }
                            else
                            {
                                searchResult = await _youTubeRepository.SearchForVideoById(queryString, cancellationToken);

                                if (searchResult.RequestedContent != null)
                                {
                                    await CacheContent(searchResult.RequestedContent, cancellationToken);
                                }
                            }
                        }
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

        private async Task CacheContent(
            Content content,
            CancellationToken cancellationToken)
        {
            await _applicationDbContext.CachedContent.AddAsync(
                    _mapper.Map<ContentDto>(content),
                    cancellationToken)
                .AsTask();

            await _applicationDbContext.SaveChangesAsync(cancellationToken);
        }
    }
}