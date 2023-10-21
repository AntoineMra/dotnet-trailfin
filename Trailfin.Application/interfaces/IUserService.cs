using CSharpFunctionalExtensions;
using Trailfin.Application.Models;
using Trailfin.Application.Models.Users;

namespace Trailfin.Application.interfaces
{
    public interface IUserService
    {
        Task<Result> UpdateName(UpdateNameRequestDto request, int userId);

        Task<Result<RegisteredUserDto>> CreateUser(RegisterUser toRegister);

        Result<InternalAuthenticatedUserDto> Get(string email);

        Task<Result<AuthenticatedUserDto>> AuthenticateUser(AuthenticateUser toAuthenticate);
    }
}