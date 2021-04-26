using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Google.Apis.YouTube.v3;
using Mediaverse.Domain.ContentSearch.ValueObjects;

namespace Mediaverse.Infrastructure.ContentSearch.Repositories.YouTube
{
    public class YouTubeRepository
    {
        private readonly YouTubeService _youTubeService;
        private readonly IMapper _mapper;

        private const string SearchParts = "id,snippet";
        private const string RetrieveByIdParts = "id,snippet,player";
        private const string SearchType = "video";
        private const int MaxSearchResultQuantity = 1;

        public YouTubeRepository(
            YouTubeService youTubeService,
            IMapper mapper)
        {
            _youTubeService = youTubeService;
            _mapper = mapper;
        }
        
        public async Task<SearchResult> SearchForVideosByKeywords(string keywords, CancellationToken cancellationToken)
        {
            var searchRequest = _youTubeService.Search.List(SearchParts);
            searchRequest.Q = keywords;
            searchRequest.Type = SearchType;
            searchRequest.MaxResults = MaxSearchResultQuantity;
            searchRequest.VideoEmbeddable = SearchResource.ListRequest.VideoEmbeddableEnum.True__;

            var searchResult = await searchRequest.ExecuteAsync(cancellationToken);
            return _mapper.Map<SearchResult>(searchResult);
        }

        public async Task<SearchResult> SearchForVideoById(string videoId, CancellationToken cancellationToken)
        {
            var searchRequest = _youTubeService.Videos.List(RetrieveByIdParts);
            searchRequest.Id = videoId;

            var searchResult = await searchRequest.ExecuteAsync(cancellationToken);
            return _mapper.Map<SearchResult>(searchResult);
        }
    }
}