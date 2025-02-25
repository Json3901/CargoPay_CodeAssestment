using CargoPay.Application.Interfaces.Infrastructure.Persistence.Repositories;
using CargoPay.Domain.Entities;

namespace CargoPay.Infrastructure.Persistence.Repositories;

public class UnitOfWork(DatabaseContext context) : IUnitOfWork
{
    private readonly DatabaseContext _context = context;

    public IGenericRepository<User> Users { get; } = new GenericRepository<User>(context);
    public IGenericRepository<Card> Cards { get; } = new GenericRepository<Card>(context);
    public IGenericRepository<Transaction> Transactions { get; } = new GenericRepository<Transaction>(context);
    public IGenericRepository<PaymentFee> PaymentFees { get; } = new GenericRepository<PaymentFee>(context);

    public async Task<int> SaveChangesAsync() => await _context.SaveChangesAsync();

    public void Dispose() => _context.Dispose();
}