using System.Collections.Generic;
using AutoMapper;
using Mediaverse.Domain.ContentSearch.Enums;
using Mediaverse.Domain.ContentSearch.ValueObjects;
using Mediaverse.Infrastructure.Common.Services;
using SearchResult = Mediaverse.Domain.ContentSearch.ValueObjects.SearchResult;
using Thumbnail = Mediaverse.Domain.ContentSearch.ValueObjects.Thumbnail;
using YouTubeData = Google.Apis.YouTube.v3.Data;

namespace Mediaverse.Infrastructure.ContentSearch.Mapping
{
    public class YouTubeProfile : Profile
    {
        private readonly Dictionary<string, ContentType> _contentTypeMappings = new Dictionary<string, ContentType>
        {
            { "youtube#video", ContentType.Video }
        };

        public YouTubeProfile() => ConfigureMappings();

        private void ConfigureMappings()
        {
            CreateMap<YouTubeData.SearchListResponse, SearchResult>()
                .ForMember(dest => dest.PreviewList, o => o.MapFrom(src => src.Items));

            CreateMap<YouTubeData.VideoListResponse, SearchResult>()
                .ForMember(dest => dest.PreviewList, o => o.MapFrom(src => src.Items));
            
            CreateMap<YouTubeData.SearchResult, Preview>()
                .ConstructUsing(x => new Preview(
                    x.Id.VideoId,
                    MediaContentSource.YouTube,
                    _contentTypeMappings[x.Id.Kind],
                    x.Snippet.Title,
                    x.Snippet.Description,
                    new Thumbnail(
                        x.Snippet.Thumbnails.Standard.Width.GetValueOrDefault(),
                        x.Snippet.Thumbnails.Standard.Height.GetValueOrDefault(),
                        x.Snippet.Thumbnails.Standard.Url)));

            CreateMap<YouTubeData.Video, Preview>()
                .ConstructUsing(x => new Preview(
                    x.Id,
                    MediaContentSource.YouTube,
                    _contentTypeMappings[x.Kind],
                    x.Snippet.Title,
                    x.Snippet.Description,
                    new Thumbnail(
                        x.Snippet.Thumbnails.Standard.Width.GetValueOrDefault(),
                        x.Snippet.Thumbnails.Standard.Height.GetValueOrDefault(),
                        x.Snippet.Thumbnails.Standard.Url)));
        }
    }
}