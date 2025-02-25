using CargoPay.Application.Interfaces.Infrastructure.Persistence.Repositories;
using CargoPay.Infrastructure.Persistence;
using CargoPay.Infrastructure.Persistence.Initialization;
using CargoPay.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CargoPay.Infrastructure;

public static class ExtensionService
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<IUnitOfWork, UnitOfWork>();
        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

        services.AddDbContext<DatabaseContext>(options => { options.UseSqlite(configuration.GetConnectionString("DefaultConnection")); });

        services.AddScoped<DatabaseInitializer>();

        return services;
    }
}