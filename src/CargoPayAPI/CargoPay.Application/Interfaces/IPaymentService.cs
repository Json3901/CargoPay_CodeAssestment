using CargoPay.Application.Dtos.Transactions;

namespace CargoPay.Application.Interfaces;

public interface IPaymentService
{
    Task<TransactionResponse> ProcessPayment(TransactionRequest transactionRequest);
}