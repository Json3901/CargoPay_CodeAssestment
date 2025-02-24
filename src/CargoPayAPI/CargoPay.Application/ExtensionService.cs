using CargoPay.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CargoPay.Application;

public static class ExtensionService
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddHostedService<PaymentFeeService>();

        return services;
    }
}