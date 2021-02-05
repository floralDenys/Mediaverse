using System.Collections.Generic;
using AutoMapper;
using Mediaverse.Domain.ContentSearch.Entities;
using Mediaverse.Domain.ContentSearch.Enums;
using Mediaverse.Infrastructure.Common.Services;
using SearchResult = Mediaverse.Domain.ContentSearch.ValueObjects.SearchResult;
using Thumbnail = Mediaverse.Domain.ContentSearch.ValueObjects.Thumbnail;
using YouTubeData = Google.Apis.YouTube.v3.Data;

namespace Mediaverse.Infrastructure.ContentSearch.Mapping
{
    public class YouTubeProfile : Profile
    {
        private readonly IContentIdProvider _contentIdProvider;

        private readonly Dictionary<string, ContentType> _contentTypeMappings = new Dictionary<string, ContentType>
        {
            { "youtube#video", ContentType.Video }
        };

        public YouTubeProfile(IContentIdProvider contentIdProvider)
        {
            _contentIdProvider = contentIdProvider;
        }

        private void ConfigureMappings()
        {
            CreateMap<YouTubeData.SearchListResponse, SearchResult>()
                .ForMember(dest => dest.PreviewList, o => o.MapFrom(src => src.Items));

            CreateMap<YouTubeData.VideoListResponse, SearchResult>()
                .ForMember(dest => dest.PreviewList, o => o.MapFrom(src => src.Items));
            
            CreateMap<YouTubeData.SearchResult, Preview>()
                .ConstructUsing(x => new Preview(
                    _contentIdProvider.GetOrCreateInternalId(x.Id.VideoId, MediaContentSource.YouTube),
                    _contentTypeMappings[x.Id.Kind],
                    x.Snippet.Title,
                    x.Snippet.Description,
                    new Thumbnail(
                        x.Snippet.Thumbnails.Standard.Width.GetValueOrDefault(),
                        x.Snippet.Thumbnails.Standard.Height.GetValueOrDefault(),
                        x.Snippet.Thumbnails.Standard.Url)));

            CreateMap<YouTubeData.Video, Preview>()
                .ConstructUsing(x => new Preview(
                    _contentIdProvider.GetOrCreateInternalId(x.Id, MediaContentSource.YouTube),
                    ContentType.Video,
                    x.Snippet.Title,
                    x.Snippet.Description,
                    new Thumbnail(
                        x.Snippet.Thumbnails.Standard.Width.GetValueOrDefault(),
                        x.Snippet.Thumbnails.Standard.Height.GetValueOrDefault(),
                        x.Snippet.Thumbnails.Standard.Url)));
        }
    }
}