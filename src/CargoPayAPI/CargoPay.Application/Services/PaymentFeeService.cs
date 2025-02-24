using CargoPay.Application.Interfaces.Infrastructure.Persistence.Repositories;
using CargoPay.Domain.Entities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CargoPay.Application.Services;

public class PaymentFeeService(IServiceScopeFactory scopeFactory) : IHostedService, IDisposable
{
    private Timer? _timer;

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _timer = new Timer(UpdateFee, null, TimeSpan.Zero, TimeSpan.FromHours(1));
        return Task.CompletedTask;
    }

    private async void UpdateFee(object? state)
    {
        using var scope = scopeFactory.CreateScope();
        var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

        var random = new Random();
        var factor = 0.8m + (decimal)random.NextDouble() * 1.2m;

        var lastFee = (await unitOfWork.PaymentFees.GetAllAsync()).LastOrDefault();
        var newFee = lastFee?.CurrentFee * factor ?? 1.0m;

        var paymentFee = new PaymentFee { CurrentFee = newFee};
        await unitOfWork.PaymentFees.AddAsync(paymentFee);
        await unitOfWork.SaveChangesAsync();
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _timer?.Change(Timeout.Infinite, 0);
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }
}