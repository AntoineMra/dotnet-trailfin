using AutoMapper;
using Trailfin.Application.Mapping;
using Trailfin.Domain.Entitites;

namespace Trailfin.Application.Models
{
    public class RegisterUser : IMapFrom<User>
    {

        public string Email { get; set; }
        public string Password { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<RegisterUser, User>().ReverseMap();
        }
    }
}