using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Google.Apis.YouTube.v3;
using Mediaverse.Domain.ContentSearch.Entities;
using Mediaverse.Domain.JointContentConsumption.Entities;

namespace Mediaverse.Infrastructure.YouTube
{
    public class YouTubeRepository
    {
        private readonly YouTubeService _youTubeService;
        private readonly IMapper _mapper;

        private const string VideoRequestParts = "id,snippet";
        private const string Type = "video";
        private const int MaxSearchResultQuantity = 25;

        public YouTubeRepository(
            YouTubeService youTubeService,
            IMapper mapper)
        {
            _youTubeService = youTubeService;
            _mapper = mapper;
        }
        
        public async Task<IList<Preview>> SearchForVideosByKeywords(string keywords)
        {
            var searchRequest = _youTubeService.Search.List(VideoRequestParts);
            searchRequest.Q = keywords;
            searchRequest.Type = Type;
            searchRequest.MaxResults = MaxSearchResultQuantity;
            searchRequest.VideoEmbeddable = SearchResource.ListRequest.VideoEmbeddableEnum.True__;

            var searchResult = await searchRequest.ExecuteAsync();
            return _mapper.Map<IList<Preview>>(searchResult.Items);
        }
    }
}