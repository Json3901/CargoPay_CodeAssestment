using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using CargoPay.Application.Interfaces.Infrastructure.Persistence.Repositories;

namespace CargoPay.Infrastructure.Persistence.Repositories;

public class GenericRepository<T>(DatabaseContext context) : IGenericRepository<T> where T : class
{
    private readonly DatabaseContext _context = context;
    private readonly DbSet<T> _dbSet = context.Set<T>();

    public async Task<T?> GetByIdAsync(Guid id) => await _dbSet.FindAsync(id);

    public async Task<IEnumerable<T>> GetAllAsync() => await _dbSet.ToListAsync();

    public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        => await _dbSet.Where(predicate).ToListAsync();

    public async Task<T> GetLastAsync()
        => await _dbSet.LastOrDefaultAsync();

    public async Task AddAsync(T entity) => await _dbSet.AddAsync(entity);

    public void Update(T entity) => _dbSet.Update(entity);

    public void Remove(T entity) => _dbSet.Remove(entity);
}