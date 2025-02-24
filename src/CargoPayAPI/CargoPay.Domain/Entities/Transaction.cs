namespace CargoPay.Domain.Entities;

public sealed class Transaction
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid CardId { get; set; }
    public decimal Amount { get; set; }
    public decimal Fee { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public Card? Card { get; set; }
}