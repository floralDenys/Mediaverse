using System.Linq;
using AutoMapper;
using Mediaverse.Domain.JointContentConsumption.Entities;
using Mediaverse.Domain.JointContentConsumption.ValueObjects;
using Mediaverse.Infrastructure.JointContentConsumption.Repositories.Dtos;
using JointContentConsumptionContext = Mediaverse.Domain.JointContentConsumption;
using ContentSearchContext = Mediaverse.Domain.ContentSearch;

namespace Mediaverse.Infrastructure.JointContentConsumption.Mapping
{
    public class InfrastructureMapping : Profile
    {
        public InfrastructureMapping() => ConfigureMappings();
        
        private void ConfigureMappings()
        {
            CreateMap<ContentSearchContext.ValueObjects.Content, JointContentConsumptionContext.Entities.Content>()
                .ConstructUsing(src => new JointContentConsumptionContext.Entities.Content(
                    new ContentId(
                        src.Id.ExternalId,
                        (JointContentConsumptionContext.Enums.MediaContentSource) src.Id.ContentSource,
                        (JointContentConsumptionContext.Enums.MediaContentType) src.Id.ContentType),
                    src.Title,
                    new ContentPlayer(src.PlayerWidth, src.PlayerHeight, src.PlayerHtml),
                    src.Description));

            CreateMap<ContentSearchContext.ValueObjects.Content, ContentDto>()
                .ForMember(dst => dst.ExternalId, opt => opt.MapFrom(src => src.Id.ExternalId))
                .ForMember(dst => dst.ContentSource, opt => opt.MapFrom(src => src.Id.ContentSource))
                .ForMember(dst => dst.ContentType, opt => opt.MapFrom(src => src.Id.ContentType))
                .ForMember(dst => dst.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dst => dst.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dst => dst.ContentPlayerHtml, opt => opt.MapFrom(src => src.PlayerHtml))
                .ForMember(dst => dst.ContentPlayerWidth, opt => opt.MapFrom(src => src.PlayerWidth))
                .ForMember(dst => dst.ContentPlayerHeight, opt => opt.MapFrom(src => src.PlayerHeight));

            CreateMap<Playlist, PlaylistDto>()
                .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dst => dst.OwnerId, opt => opt.MapFrom(src => src.Owner.Profile.Id))
                .ForMember(dst => dst.IsTemporary, opt => opt.MapFrom(src => src.IsTemporary))
                .ForMember(dst => dst.CurrentlyPlayingContentIndex,
                    opt => opt.MapFrom(src => src.CurrentlyPlayingContentIndex))
                .ForMember(dst => dst.PlaylistItems, opt => opt.MapFrom(src => src.ToList()));

            CreateMap<PlaylistItem, PlaylistItemDto>()
                .ForMember(dst => dst.ExternalId, opt => opt.MapFrom(src => src.ContentId.ExternalId))
                .ForMember(dst => dst.ContentSource, opt => opt.MapFrom(src => src.ContentId.ContentSource))
                .ForMember(dst => dst.ContentType, opt => opt.MapFrom(src => src.ContentId.ContentType))
                .ForMember(dst => dst.PlaylistItemIndex, opt => opt.MapFrom(src => src.PlaylistItemIndex));

            CreateMap<Room, RoomDto>()
                .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dst => dst.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dst => dst.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dst => dst.HostId, opt => opt.MapFrom(src => src.Host.Profile.Id))
                .ForMember(dst => dst.Token, opt => opt.MapFrom(src => src.Invitation.Token))
                .ForMember(dst => dst.ActivePlaylistId, opt => opt.MapFrom(src => src.ActivePlaylistId))
                .ForMember(dst => dst.MaxViewersQuantity, opt => opt.MapFrom(src => src.MaxViewersQuantity));

            CreateMap<Viewer, ViewerDto>()
                .ForMember(src => src.Id, opt => opt.MapFrom(src => src.Profile.Id));
        }
    }
}