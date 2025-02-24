namespace CargoPay.Domain.Entities;

public class PaymentFee
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public decimal CurrentFee { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}