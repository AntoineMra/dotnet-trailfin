using AutoMapper;
using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Trailfin.Application.interfaces;
using Trailfin.Application.Models;
using Trailfin.Application.Models.Helpers;
using Trailfin.Application.Models.Users;
using Trailfin.Domain.Entitites;
using Trailfin.Infrastructure.Persistence;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Trailfin.Application.Services
{
    public class UserService : IUserService
    {
        private readonly TrailfinContext _context;
        private readonly IMapper _mapper;
        private readonly AppSettings _appSettings;

        public UserService(IOptions<AppSettings> appSettings, TrailfinContext context, IMapper mapper)
        {
            _appSettings = appSettings.Value;
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result<AuthenticatedUserDto>> AuthenticateUser(AuthenticateUser toAuthenticate)
        {
            string hashed = string.Empty;
            using (SHA256 sha256Hash = SHA256.Create())
            {
                hashed = GetHash(sha256Hash, toAuthenticate.Password);
            }
            if (hashed == null && hashed == string.Empty)
                return Result.Failure<AuthenticatedUserDto>("Error while manipulation to given user to authentificate");
            User exist = await _context.Users.FirstOrDefaultAsync(user => user.Email == toAuthenticate.Email && user.Password == hashed);
            if (exist == null)
                return Result.Failure<AuthenticatedUserDto>("User with given email & passord doesn't exist");

            AuthenticatedUserDto authenticatedUser = _mapper.Map<AuthenticatedUserDto>(exist);
            return Result.Success(authenticatedUser);
        }

        public async Task<Result<RegisteredUserDto>> CreateUser(RegisterUser toRegister)
        {
            User exist = await _context.Users.FirstOrDefaultAsync(user => user.Email == toRegister.Email);
            if (exist != null)
                return Result.Failure<RegisteredUserDto>("User with given email already exist");
            string hashed = string.Empty;
            using (SHA256 sha256Hash = SHA256.Create())
            {
                hashed = GetHash(sha256Hash, toRegister.Password);
            }
            if (hashed == null && hashed == string.Empty)
                return Result.Failure<RegisteredUserDto>("Error while manipulation to given user to register");
            toRegister.Password = hashed;
            User toCreate = _mapper.Map<User>(toRegister);

            string token = generateJwtToken(toCreate);
            if (token == null)
                return Result.Failure<RegisteredUserDto>("Could not generate token wiht given user");
            _context.Users.Add(toCreate);
            await _context.SaveChangesAsync();

            RegisteredUserDto registered = _mapper.Map<RegisteredUserDto>(toCreate);
            registered.Token= token;
            return Result.Success(registered);
        }

        private string generateJwtToken(User user)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[] { new Claim("email", user.Email.ToString()) }),
                    Expires = DateTime.UtcNow.AddDays(7),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                return tokenHandler.WriteToken(token);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private static string GetHash(HashAlgorithm hashAlgorithm, string input)
        {
            byte[] data = hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(input));
            var sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            return sBuilder.ToString();
        }

        public Result<InternalAuthenticatedUserDto> Get(string email)
        {
            User user = _context.Users.FirstOrDefault(user => user.Email == email);
            if (user == null)
                return Result.Failure<InternalAuthenticatedUserDto>("Could not find user");
            InternalAuthenticatedUserDto response = _mapper.Map<InternalAuthenticatedUserDto>(user);
            if (response == null)
                return Result.Failure<InternalAuthenticatedUserDto>("Could not map user into Authenticate response");
            return Result.Success(response);
        }

        public async Task<Result> UpdateName(UpdateNameRequestDto request, int userId)
        {
            if (request.FirstName == null || request.Pseudo == null || request.LastName == null)
                return Result.Failure("One or more properties given is null : pseudo, lastname, firstname.");
            User pseudoExist = await _context.Users.FirstOrDefaultAsync(user => user.Pseudo == request.Pseudo);
            if (pseudoExist != null)
                return Result.Failure("Pseudo given already exist.");
            User user = await _context.Users.FirstOrDefaultAsync(user => user.Id == userId);
            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.Pseudo = request.Pseudo;

            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return Result.Success();
        }          

    }
}