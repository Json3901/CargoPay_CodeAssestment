using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CargoPay.Application.Interfaces;
using CargoPay.Application.Interfaces.Infrastructure.Persistence.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;

namespace CargoPay.Application.Services;

public class AuthService(IHttpContextAccessor httpContextAccessor, IUnitOfWork unitOfWork)
    : IAuthService
{
    public async Task<string> GetSession()
    {
        return await Task.FromResult(
            httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier) !
                .Value);
    }

    public async Task<string> LoginAsync(string username, string password)
    {
        var user = (await unitOfWork.Users.FindAsync(x => x.Username == username)).FirstOrDefault();

        if (user is null)
        {
            throw new Exception("User not Found");
        }

        if (!BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
        {
            throw new Exception("Invalid Password");
        }

        return GenerateJwt(username);
    }

    private string GenerateJwt(string username)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, username)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("superSecretKey@345"));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        var tokenDescription = new SecurityTokenDescriptor()
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.Now.AddDays(1),
            SigningCredentials = credentials,
            Audience = "",
            Issuer = ""
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescription);

        return tokenHandler.WriteToken(token);
    }
}