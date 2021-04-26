﻿using AutoMapper;
using Mediaverse.Application.ContentSearch.Queries.GetRelevantContent.Dtos;
using Mediaverse.Domain.ContentSearch.Entities;
using Mediaverse.Domain.ContentSearch.ValueObjects;

namespace Mediaverse.Infrastructure.ContentSearch.Mapping
{
    public class ApplicationProfile : Profile
    {
        public ApplicationProfile()
        {
            ConfigureMappings();
        }

        private void ConfigureMappings()
        {
            CreateMap<Preview, PreviewDto>();
            CreateMap<ContentId, ContentIdDto>();
            CreateMap<Thumbnail, ThumbnailDto>();
            CreateMap<SearchResult, SearchResultDto>()
                .ForMember(dst => dst.Previews, opt => opt.MapFrom(src => src.MatchingContentPreviews));
        }
    }
}