using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CargoPay.Application.Dtos.Users;
using CargoPay.Application.Interfaces;
using CargoPay.Application.Interfaces.Infrastructure.Persistence.Repositories;
using CargoPay.Domain.Entities;
using CargoPay.Domain.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;

namespace CargoPay.Application.Services;

public class UserService(IHttpContextAccessor httpContextAccessor, IUnitOfWork unitOfWork)
    : IUserService
{
    public async Task<User?> Authenticated()
    {
        var authenticatedUsername = await GetSession();
        var authenticatedUser =
            (await unitOfWork.Users.FindAsync(x => x.Username == authenticatedUsername))
            .FirstOrDefault();

        if (authenticatedUser == null)
        {
            throw new Exception("Unauthorized.");
        }

        return authenticatedUser;
    }

    public async Task<string> LoginAsync(LoginUser loginUser)
    {
        var user = (await unitOfWork.Users.FindAsync(x => x.Username == loginUser.Username)).FirstOrDefault();

        if (user is null)
        {
            throw new Exception("User not Found");
        }

        if (!BCrypt.Net.BCrypt.Verify(loginUser.Password, user.PasswordHash))
        {
            throw new Exception("Invalid Password");
        }

        return GenerateJwt(loginUser.Username);
    }

    public async Task<string> CreateUserAsync(LoginUser loginUser)
    {
        var authenticatedUser = await Authenticated();

        if (authenticatedUser is null)
        {
            throw new Exception("Unauthorized.");
        }

        var user = (await unitOfWork.Users.FindAsync(x => x.Username == loginUser.Username)).FirstOrDefault();

        if (user is not null)
        {
            throw new Exception("User already exists.");
        }

        await unitOfWork.Users.AddAsync(new User
        {
            Username = loginUser.Username,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(loginUser.Password),
            Role = Role.Client
        });

        await unitOfWork.SaveChangesAsync();

        return "User has been created successfully.";
    }

    private async Task<string> GetSession()
    {
        return await Task.FromResult(
            httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier) !
                .Value);
    }


    private string GenerateJwt(string username)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, username)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
            "p2l+HnBQzJ6RQKHcJv3cVJTR1qOpe45i3Qf1tD+xE6XzYp2W1X5PrRjNw3Z5VgPvO93IvP7RHF2HZ/wXlg5HtQ=="));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        var tokenDescription = new SecurityTokenDescriptor()
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.Now.AddDays(1),
            SigningCredentials = credentials,
            Audience = "C4rg0P4y",
            Issuer = "C4rg0P4y"
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescription);

        return tokenHandler.WriteToken(token);
    }
}