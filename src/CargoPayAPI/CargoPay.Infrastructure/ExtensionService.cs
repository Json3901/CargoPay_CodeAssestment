using CargoPay.Application.Interfaces.Infrastructure.Persistence.Repositories;
using CargoPay.Infrastructure.Persistence;
using CargoPay.Infrastructure.Persistence.Initialization;
using CargoPay.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CargoPay.Infrastructure;

public static class ExtensionService
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddTransient<IUnitOfWork, UnitOfWork>();
        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

        services.AddDbContext<DatabaseContext>(options => { options.UseSqlite("Data Source=CargoPay.db"); });

        services.AddScoped<DatabaseInitializer>();

        return services;
    }
}