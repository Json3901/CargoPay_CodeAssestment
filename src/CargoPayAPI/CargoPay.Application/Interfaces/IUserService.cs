using CargoPay.Application.Dtos.Users;
using CargoPay.Domain.Entities;

namespace CargoPay.Application.Interfaces;

public interface IUserService
{
    Task<string> LoginAsync(LoginUser loginUser);
    Task<string> CreateUserAsync(LoginUser loginUser);
    Task<User?> Authenticated();
}