using AutoMapper;
using Mediaverse.Domain.ContentSearch.Entities;
using Mediaverse.Domain.ContentSearch.ValueObjects;
using Mediaverse.Infrastructure.ContentSearch.Repositories.Dtos;

namespace Mediaverse.Infrastructure.ContentSearch.Mapping
{
    public class InfrastructureProfile : Profile
    {
        public InfrastructureProfile()
        {
            ConfigureMappings();
        }

        private void ConfigureMappings()
        {
            CreateMap<Content, ContentDto>()
                .ForMember(dst => dst.ExternalId, opt => opt.MapFrom(src => src.Id.ExternalId))
                .ForMember(dst => dst.ContentSource, opt => opt.MapFrom(src => src.Id.ContentSource))
                .ForMember(dst => dst.ContentType, opt => opt.MapFrom(src => src.Id.ContentType))
                .ForMember(dst => dst.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dst => dst.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dst => dst.ContentPlayerHtml, opt => opt.MapFrom(src => src.PlayerHtml))
                .ForMember(dst => dst.ContentPlayerWidth, opt => opt.MapFrom(src => src.PlayerWidth))
                .ForMember(dst => dst.ContentPlayerHeight, opt => opt.MapFrom(src => src.PlayerHeight));

            CreateMap<ContentDto, Content>()
                .ConstructUsing(src => new Content(
                    new ContentId(
                        src.ExternalId,
                        src.ContentSource,
                        src.ContentType),
                    src.Title,
                    new Thumbnail(
                        src.ThumbnailWidth,
                        src.ThumbnailHeight,
                        src.ThumbnailUrl),
                    src.ContentPlayerHtml,
                    src.ContentPlayerWidth,
                    src.ContentPlayerHeight,
                    src.Description));
        }
    }
}