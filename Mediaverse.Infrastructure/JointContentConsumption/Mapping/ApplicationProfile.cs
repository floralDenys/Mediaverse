using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Mediaverse.Application.JointContentConsumption.Commands.SwitchContent.Dtos;
using Mediaverse.Application.JointContentConsumption.Common.Dtos;
using Mediaverse.Application.JointContentConsumption.Queries.GetPlaylist.Dtos;
using Mediaverse.Domain.JointContentConsumption.Entities;
using Mediaverse.Domain.JointContentConsumption.ValueObjects;
using PlaylistDto = Mediaverse.Application.JointContentConsumption.Common.Dtos.PlaylistDto;
using PlaylistItemDto = Mediaverse.Application.JointContentConsumption.Common.Dtos.PlaylistItemDto;
using RoomDto = Mediaverse.Application.JointContentConsumption.Common.Dtos.RoomDto;

namespace Mediaverse.Infrastructure.JointContentConsumption.Mapping
{
    public class ApplicationProfile : Profile
    {
        public ApplicationProfile() => ConfigureMappings();

        private void ConfigureMappings()
        {
            CreateMap<ContentIdDto, ContentId>()
                .ConstructUsing(x => new ContentId(
                    x.ExternalId,
                    x.ContentSource,
                    x.ContentType));
            
            CreateMap<ContentId, ContentIdDto>()    
                .ForMember(dst => dst.ExternalId, opt => opt.MapFrom(src => src.ExternalId))
                .ForMember(dst => dst.ContentSource, opt => opt.MapFrom(src => src.ContentSource))
                .ForMember(dst => dst.ContentType, opt => opt.MapFrom(src => src.ContentType));

            CreateMap<Playlist, PlaylistDto>()
                .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Items, o => o.MapFrom(src => src.ToList()));

            CreateMap<PlaylistItem, PlaylistItemDto>();
            
            CreateMap<Playlist, SelectablePlaylistDto>();

            CreateMap<Content, PlaylistItemDto>()
                .ForMember(dest => dest.Title, o => o.MapFrom(src => src.Title))
                .ForMember(dest => dest.Description, o => o.MapFrom(src => src.Description));

            CreateMap<Content, ContentDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id));
            CreateMap<ContentPlayer, ContentPlayerDto>();

            CreateMap<Room, RoomDto>()
                .ForMember(dst => dst.Token, opt => opt.MapFrom(src => src.Invitation.Token))
                .ForMember(dst => dst.MaxViewersQuantity, opt => opt.MapFrom(src => src.MaxViewersQuantity))
                .ForMember(dst => dst.CurrentViewersQuantity, opt => opt.MapFrom(src => src.Viewers.Count));

            CreateMap<IEnumerable<Viewer>, AffectedViewersDto>()
                .ForMember(dst => dst.ViewerIds, opt => opt.MapFrom(src => src.Select(v => v.Profile.Id)));
        }
    }
}