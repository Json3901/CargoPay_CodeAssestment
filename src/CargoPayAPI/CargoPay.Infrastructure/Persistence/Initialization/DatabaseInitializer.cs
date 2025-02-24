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
        if (!await _context.PaymentFees.AnyAsync())
        {
            _context.PaymentFees.Add(new PaymentFee { CurrentFee = 1 });
            await _context.SaveChangesAsync();
        }
    }
}