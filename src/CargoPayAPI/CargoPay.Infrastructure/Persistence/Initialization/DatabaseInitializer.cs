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
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes("12345");
            var hash = sha256.ComputeHash(bytes);

            _context.Users.Add(new User
            {
                Username = "Admin",
                PasswordHash = Convert.ToBase64String(hash),
                Role = Domain.Enums.Role.Admin
            });
            await _context.SaveChangesAsync();
        }
    }
}