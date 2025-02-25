using CargoPay.Application.Dtos.Transactions;
using CargoPay.Application.Interfaces;
using CargoPay.Application.Interfaces.Infrastructure.Persistence.Repositories;
using CargoPay.Domain.Entities;
using CargoPay.Domain.Enums;

namespace CargoPay.Application.Services;

public class PaymentService(IUnitOfWork unitOfWork, ICardService cardService) : IPaymentService
{
    public async Task<TransactionResponse> ProcessPayment(TransactionRequest transactionRequest)
    {
        var card = await cardService.GetCardByNumberAsync(transactionRequest.CardNumber);

        var fee = (await unitOfWork.PaymentFees.GetAllAsync()).OrderBy(x => x.CreatedAt).LastOrDefault();
        var feeAmount = transactionRequest.Amount * fee.CurrentFee;
        var totalAmount = Math.Round(transactionRequest.Amount + feeAmount,2);

        var transaction = new Transaction(
            (await unitOfWork.Cards.FindAsync(x => x.CardNumber == transactionRequest.CardNumber)).FirstOrDefault().Id,
            transactionRequest.Amount, feeAmount);

        await unitOfWork.Transactions.AddAsync(transaction);
        await unitOfWork.SaveChangesAsync();

        if (card.Balance < totalAmount)
        {
            transaction.Status = TransactionStatus.Declined;
            unitOfWork.Transactions.Update(transaction);
            await unitOfWork.SaveChangesAsync();
            throw new Exception("Declined Transaction.Insufficient funds.");
        }

        card.Balance -= totalAmount;
        try
        {
            await cardService.UpdateBalanceAsync(card.CardNumber, card.Balance);
        }
        catch (Exception e)
        {
            transaction.Status = TransactionStatus.Declined;
            unitOfWork.Transactions.Update(transaction);
            await unitOfWork.SaveChangesAsync();
            throw new Exception("Unauthorized. Declined Transaction. Balance was not modified.");
        }

        transaction.Status = TransactionStatus.Approved;
        unitOfWork.Transactions.Update(transaction);
        await unitOfWork.SaveChangesAsync();

        return new TransactionResponse
        {
            TransactionId = transaction.Id.ToString(),
            CardNumber = card.CardNumber,
            Username = card.Username,
            Amount = transactionRequest.Amount,
            Fee = feeAmount,
            CardBalance = card.Balance,
            Status = transaction.Status.ToString()
        };
    }
}