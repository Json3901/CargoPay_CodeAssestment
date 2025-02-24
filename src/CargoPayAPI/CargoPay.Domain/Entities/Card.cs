namespace CargoPay.Domain.Entities;

public class Card(string cardNumber)
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string CardNumber { get; set; } = cardNumber;
    public decimal Balance { get; set; } = 100;
}