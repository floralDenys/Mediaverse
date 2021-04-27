using System.Linq;
using AutoMapper;
using Mediaverse.Application.JointContentConsumption.Commands.SwitchContent.Dtos;
using Mediaverse.Application.JointContentConsumption.Common.Dtos;
using Mediaverse.Application.JointContentConsumption.Queries.GetPlaylist.Dtos;
using Mediaverse.Domain.JointContentConsumption.Entities;
using Mediaverse.Domain.JointContentConsumption.ValueObjects;

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

            CreateMap<Playlist, PlaylistDto>()
                .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Items, o => o.MapFrom(src => src.ToList()));

            CreateMap<PlaylistItem, PlaylistItemDto>();
            
            CreateMap<Playlist, SelectablePlaylistDto>();

            CreateMap<Content, PlaylistItemDto>()
                .ForMember(dest => dest.Title, o => o.MapFrom(src => src.Title))
                .ForMember(dest => dest.Description, o => o.MapFrom(src => src.Description));

            CreateMap<Content, ContentDto>();
            CreateMap<ContentPlayer, ContentPlayerDto>();

            CreateMap<Room, RoomDto>();
        }
    }
}