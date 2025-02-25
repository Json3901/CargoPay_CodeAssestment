using CargoPay.Domain.Enums;

namespace CargoPay.Domain.Entities;

public sealed class Transaction(Guid cardId, decimal amount, decimal fee)
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid CardId { get; set; } = cardId;
    public decimal Amount { get; set; } = amount;
    public decimal Fee { get; set; } = fee;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public TransactionStatus Status { get; set; } = TransactionStatus.Pending;
    public Card? Card { get; set; }
}