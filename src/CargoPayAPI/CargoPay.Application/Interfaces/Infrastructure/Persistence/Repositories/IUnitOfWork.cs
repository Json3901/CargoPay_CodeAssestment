using CargoPay.Domain.Entities;

namespace CargoPay.Application.Interfaces.Infrastructure.Persistence.Repositories;

public interface IUnitOfWork: IDisposable
{
    IGenericRepository<User> Users { get; }
    IGenericRepository<Card> Cards { get; }
    IGenericRepository<Transaction> Transactions { get; }
    IGenericRepository<PaymentFee> PaymentFees { get; }
    Task<int> SaveChangesAsync();
}