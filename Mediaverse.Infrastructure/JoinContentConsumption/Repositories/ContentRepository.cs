using System;
using Mediaverse.Domain.JointContentConsumption.Entities;
using Mediaverse.Domain.JointContentConsumption.Enums;
using Mediaverse.Domain.JointContentConsumption.Repositories;
using Mediaverse.Infrastructure.Common.Enums;
using Mediaverse.Infrastructure.Common.Services;
using Mediaverse.Infrastructure.YouTube;

namespace Mediaverse.Infrastructure.JoinContentConsumption.Repositories
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
        
        public Content SearchForContent(MediaContentSource source, string queryString)
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