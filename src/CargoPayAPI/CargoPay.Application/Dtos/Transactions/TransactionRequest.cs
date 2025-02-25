namespace CargoPay.Application.Dtos.Transactions;

public class TransactionRequest
{
    public string CardNumber { get; set; }
    public decimal Amount { get; set; }
}