using CSharpFunctionalExtensions;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Trailfin.Application.interfaces;
using Trailfin.Application.Models;
using Trailfin.Application.Models.Helpers;
using Trailfin.Application.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace Trailfin.Web.Controllers.Auth
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly AppSettings _appSettings;

        public JwtMiddleware(RequestDelegate next, IOptions<AppSettings> appSettings)
        {
            _next = next;
            _appSettings = appSettings.Value;
        }

        public async Task Invoke(HttpContext context, IUserService userService)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            if (token != null)
                attachUserToContext(context, userService, token);
            await _next(context);
        }

        private async Task attachUserToContext(HttpContext context, IUserService userService, string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var userEmail = jwtToken.Claims.First(x => x.Type == "email").Value;
                Result<InternalAuthenticatedUserDto> usr = userService.Get(userEmail);
                context.Items["User"] = usr.Value;
            }
            catch
            {
                // Ne fais rien si l'user n'existe pas ou il n'est pas authentifier, aucun user n'est a attacher a l'application
            }
        }
    }
}