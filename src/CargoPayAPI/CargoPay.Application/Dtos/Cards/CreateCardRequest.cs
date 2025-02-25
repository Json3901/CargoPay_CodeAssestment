namespace CargoPay.Application.Dtos.Cards;

public class CreateCardRequest
{
    public string Username { get; set; }
    public decimal AuthorizedAmount { get; set; }
}