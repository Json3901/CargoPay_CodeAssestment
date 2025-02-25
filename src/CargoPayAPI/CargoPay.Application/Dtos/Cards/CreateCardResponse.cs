namespace CargoPay.Application.Dtos.Cards;

public class CreateCardResponse
{
    public string Username { get; set; }
    public string CardNumber { get; set; }
    public decimal Balance { get; set; }
}