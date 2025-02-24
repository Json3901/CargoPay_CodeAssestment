namespace CargoPay.Domain.Entities;

public class PaymentFee
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public decimal CurrentFee { get; set; }
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}