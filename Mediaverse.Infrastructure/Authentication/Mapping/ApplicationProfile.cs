using AutoMapper;
using Mediaverse.Application.Authentication.Common.Dtos;
using Mediaverse.Domain.Authentication.Entities;

namespace Mediaverse.Infrastructure.Authentication.Mapping
{
    public class ApplicationProfile : Profile
    {
        public ApplicationProfile() => ConfigureMapping();
        
        private void ConfigureMapping()
        {
            CreateMap<User, UserDto>();
        }
    }
}