using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Mvc;
using Trailfin.Web.Controllers.Auth;
using Trailfin.Application.interfaces;
using Trailfin.Application.Models;
using Trailfin.Application.Models.Users;
using Trailfin.Domain.Entitites;

namespace Trailfin.Web.Controllers
{
    public class AuthController : BaseApiController
    {
        private readonly IUserService _userService;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly InternalAuthenticatedUserDto? _contextUser;

        public AuthController(IUserService userService, IHttpContextAccessor contextAccessor)
        {
            _userService = userService;
            _contextAccessor = contextAccessor;
            _contextUser = (InternalAuthenticatedUserDto)_contextAccessor.HttpContext.Items["User"];
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterUser user)
        {
            Result<RegisteredUserDto> res = await _userService.CreateUser(user);
            if (res.IsFailure)
                return BadRequest(res.Error);
            return Ok(res.Value);
        }

        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate(AuthenticateUser toAuthenticate)
        {
            Result<AuthenticatedUserDto> res = await _userService.AuthenticateUser(toAuthenticate);
            if (res.IsFailure)
                return BadRequest(res.Error);
            return Ok(res.Value);
        }

        [Authorize]
        [HttpPut("name/{userId}")]
        public async Task<IActionResult> UpdateName([FromBody] UpdateNameRequestDto request, int userId)
        {
            if (_contextUser.Id != userId)
                BadRequest("UserId sent is wrong.");
            Result res = await _userService.UpdateName(request, userId);
            if (res.IsFailure)
                return BadRequest(res.Error);
            return Ok();
        }
    }
}