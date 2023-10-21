using AutoMapper;
using Trailfin.Application.Mapping;
using Trailfin.Domain.Entitites;

namespace Trailfin.Application.Models
{
    public class AuthenticatedUserDto : IMapFrom<User>
    {
        public int Id { get; set; }
        public string Email { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<AuthenticatedUserDto, User>().ReverseMap();
        }
    }
}