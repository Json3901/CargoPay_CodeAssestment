using System.Security.Cryptography;
using System.Text;
using CargoPay.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CargoPay.Infrastructure.Persistence.Initialization;

public class DatabaseInitializer
{
    private readonly DatabaseContext _context;

    public DatabaseInitializer(DatabaseContext context)
    {
        _context = context;
    }

    public async Task InitializeAsync()
    {
        await _context.Database.MigrateAsync();
        await SeedDataAsync();
    }

    private async Task SeedDataAsync()
    {
        if (!await _context.Users.AnyAsync())
        {
            var hash = BCrypt.Net.BCrypt.HashPassword("12345");

            _context.Users.Add(new User
            {
                Username = "Admin",
                PasswordHash = hash,
                Role = Domain.Enums.Role.Admin
            });
            await _context.SaveChangesAsync();
        }
    }
}