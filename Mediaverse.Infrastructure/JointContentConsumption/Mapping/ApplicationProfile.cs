using System.Linq;
using AutoMapper;
using Mediaverse.Application.JointContentConsumption.Common.Dtos;
using Mediaverse.Domain.JointContentConsumption.Entities;

namespace Mediaverse.Infrastructure.JointContentConsumption.Mapping
{
    public class ApplicationProfile : Profile
    {
        public ApplicationProfile() => ConfigureMappings();

        private void ConfigureMappings()
        {
            CreateMap<ContentIdDto, ContentId>();

            CreateMap<Playlist, PlaylistDto>()
                .ForMember(dest => dest.Items, o => o.MapFrom(src => src.ToList()));

            CreateMap<Content, PlaylistItemDto>()
                .ForMember(dest => dest.Title, o => o.MapFrom(src => src.Title))
                .ForMember(dest => dest.Description, o => o.MapFrom(src => src.Description));
        }
    }
}