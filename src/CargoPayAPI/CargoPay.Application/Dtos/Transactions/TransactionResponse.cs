namespace CargoPay.Application.Dtos.Transactions;

public class TransactionResponse
{
    public string TransactionId { get; set; }
    public string CardNumber { get; set; }
    public string Username { get; set; }
    public decimal Amount { get; set; }
    public decimal Fee { get; set; }
    public decimal CardBalance { get; set; }
    public string Status { get; set; }
}