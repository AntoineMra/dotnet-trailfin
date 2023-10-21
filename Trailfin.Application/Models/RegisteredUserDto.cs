using AutoMapper;
using Trailfin.Application.Mapping;
using Trailfin.Domain.Entitites;

namespace Trailfin.Application.Models
{
    public class RegisteredUserDto : IMapFrom<User>
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<RegisteredUserDto, User>().ReverseMap();
        }
    }
}