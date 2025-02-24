using CargoPay.Infrastructure.Persistence;
using CargoPay.Infrastructure.Persistence.Initialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CargoPay.Infrastructure;

public static class ExtensionService
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddDbContext<DatabaseContext>(options =>
        {
            options.UseSqlite("Data Source=CargoPay.db");
        });

        services.AddScoped<DatabaseInitializer>();

        return services;
    }
}