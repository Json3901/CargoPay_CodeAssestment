namespace CargoPay.Domain.Entities;

public class Card(Guid userId, string cardNumber)
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid UserId { get; set; } = userId;
    public string CardNumber { get; set; } = cardNumber;
    public decimal Balance { get; set; } = 100;

}