using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Mediaverse.Domain.JointContentConsumption.Entities;
using Mediaverse.Domain.JointContentConsumption.Repositories;
using Mediaverse.Domain.JointContentConsumption.ValueObjects;
using Mediaverse.Infrastructure.Common.Persistence;
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
            var searchResult = await _contentRepository.SearchForContentAsync(
                (ContentSearchContext.Enums.MediaContentSource) contentId.ContentSource,
                ContentSearchContext.Enums.ContentQueryType.ContentId,
                contentId.ExternalId,
                cancellationToken);

            return _mapper.Map<Content>(searchResult.RequestedContent);;
        }
    }
}