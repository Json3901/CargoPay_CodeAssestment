using CargoPay.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CargoPay.Infrastructure.Persistence;

public class DatabaseContext(DbContextOptions<DatabaseContext> options) : DbContext(options)
{
    public DbSet<Card> Cards { get; set; }
    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<PaymentFee> PaymentFees { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Card>()
            .HasIndex(c => c.CardNumber)
            .IsUnique();

    }
}