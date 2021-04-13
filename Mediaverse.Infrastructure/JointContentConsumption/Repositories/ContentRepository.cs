using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Mediaverse.Domain.JointContentConsumption.Entities;
using Mediaverse.Domain.JointContentConsumption.Repositories;
using Mediaverse.Domain.JointContentConsumption.ValueObjects;
using Search = Mediaverse.Domain.ContentSearch;

namespace Mediaverse.Infrastructure.JointContentConsumption.Repositories
{
    public class ContentRepository : IContentRepository
    {
        private readonly Search.Repositories.IContentRepository _contentRepository;

        private readonly IMapper _mapper;

        public ContentRepository(
            Search.Repositories.IContentRepository contentRepository,
            IMapper mapper)
        {
            _contentRepository = contentRepository;
            _mapper = mapper;
        }

        public async Task<Content> GetAsync(ContentId contentId, CancellationToken cancellationToken)
        {
            var searchResult = await _contentRepository.SearchForContentAsync(
                (Search.Enums.MediaContentSource) contentId.ContentSource,
                Search.Enums.ContentQueryType.ContentId,
                contentId.ExternalId,
                cancellationToken);

            return _mapper.Map<Content>(searchResult.RequestedContent);
        }
    }
}