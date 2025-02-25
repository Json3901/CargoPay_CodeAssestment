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
        _timer = new Timer(UpdateFee, null, TimeSpan.Zero, TimeSpan.FromSeconds(30));
        return Task.CompletedTask;
    }

    private async void UpdateFee(object? state)
    {
        using var scope = scopeFactory.CreateScope();
        var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

        var random = new Random();
        var factor = Math.Round((decimal)(random.NextDouble() * 2), 2);

        if (factor < 0.01m)
        {
            factor = 0.01m;
        }

        var lastFee = (await unitOfWork.PaymentFees.GetAllAsync())
            .ToList().LastOrDefault();
        var newFee = lastFee?.CurrentFee * factor ?? 1.0m;

        var paymentFee = new PaymentFee { CurrentFee = newFee };
        await unitOfWork.PaymentFees.AddAsync(paymentFee);
        await unitOfWork.SaveChangesAsync();
        Console.WriteLine(paymentFee.CurrentFee);
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