using CargoPay.Application.Interfaces;
using CargoPay.Application.Interfaces.Infrastructure.Persistence.Repositories;
using CargoPay.Domain.Entities;

namespace CargoPay.Application.Services;

public class PaymentService(IUnitOfWork unitOfWork, ICardService cardService)
{
    public async Task ProcessPayment(string cardNumber, decimal amount)
    {
        var card = await cardService.GetCardByNumberAsync(cardNumber);
        if (card == null)
            throw new Exception("Card not found");

        var fee = await unitOfWork.PaymentFees.GetLastAsync();
        var feeAmount = amount * fee.CurrentFee;
        var totalAmount = amount + feeAmount;

        if (card.Balance < totalAmount)
            throw new Exception("Card has insufficient funds");

        card.Balance -= totalAmount;
        var updatedBalance = await cardService.UpdateBalanceAsync(cardNumber, card.Balance);

        if (!updatedBalance)
        {
            throw new Exception("Declined Transaction. Failed to update card balance");
        }

        var transaction = new Transaction
        {
            CardId = card.Id,
            Amount = amount,
            Fee = feeAmount
        };

        unitOfWork.Transactions.Update(transaction);
        await unitOfWork.SaveChangesAsync();
    }
}