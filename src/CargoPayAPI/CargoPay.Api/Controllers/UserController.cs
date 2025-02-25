using CargoPay.Application.Dtos.Users;
using CargoPay.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CargoPay.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController(IUserService userService) : ControllerBase
{
    [HttpPost("login", Name = "Login")]
    public async Task<IActionResult> Login(LoginUser loginUser)
    {
        return Ok(await userService.LoginAsync(loginUser));
    }

    [Authorize]
    [HttpPost("create", Name = "createUser")]
    public async Task<IActionResult> CreateUser(LoginUser loginUser)
    {
        return Ok(await userService.CreateUserAsync(loginUser));
    }
}