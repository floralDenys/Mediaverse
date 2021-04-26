using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Mediaverse.Domain.JointContentConsumption.Entities;
using Mediaverse.Domain.JointContentConsumption.Repositories;
using Mediaverse.Domain.JointContentConsumption.ValueObjects;
using Mediaverse.Infrastructure.Common.Persistence;
using Mediaverse.Infrastructure.JointContentConsumption.Repositories.Dtos;
using ContentSearchContext = Mediaverse.Domain.ContentSearch;

namespace Mediaverse.Infrastructure.JointContentConsumption.Repositories
{
    public class ContentRepository : IContentRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;
        
        private readonly ContentSearchContext.Repositories.IContentRepository _contentRepository;

        private readonly IMapper _mapper;

        public ContentRepository(
            ApplicationDbContext applicationDbContext,
            ContentSearchContext.Repositories.IContentRepository contentRepository,
            IMapper mapper)
        {
            _applicationDbContext = applicationDbContext;
            _contentRepository = contentRepository;
            _mapper = mapper;
        }

        public async Task<Content> GetAsync(ContentId contentId, CancellationToken cancellationToken)
        {
            Content requestedContent;
            
            var cachedContent = await _applicationDbContext.CachedContent.FindAsync(
                contentId.ExternalId, contentId.ContentType, contentId.ContentSource);

            if (cachedContent != null)
            {
                requestedContent = _mapper.Map<Content>(cachedContent);
            }
            else
            {
                var searchResult = await _contentRepository.SearchForContentAsync(
                    (ContentSearchContext.Enums.MediaContentSource) contentId.ContentSource,
                    ContentSearchContext.Enums.ContentQueryType.ContentId,
                    contentId.ExternalId,
                    cancellationToken);

                if (searchResult.RequestedContent != null)
                {
                    await CacheContent(searchResult.RequestedContent, cancellationToken);
                }
                
                requestedContent = _mapper.Map<Content>(searchResult.RequestedContent);
            }

            return requestedContent;
        }

        private Task CacheContent(
            ContentSearchContext.ValueObjects.Content content,
            CancellationToken cancellationToken
            ) =>
            _applicationDbContext.CachedContent.AddAsync(
                _mapper.Map<ContentDto>(content),
                cancellationToken)
                .AsTask();
    }
}