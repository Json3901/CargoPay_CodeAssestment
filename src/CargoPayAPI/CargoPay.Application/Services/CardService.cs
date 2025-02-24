using CargoPay.Application.Interfaces;
using CargoPay.Application.Interfaces.Infrastructure.Persistence.Repositories;
using CargoPay.Domain.Entities;

namespace CargoPay.Application.Services;

public class CardService(IUnitOfWork unitOfWork) : ICardService
{
    public async Task<Card> CreateCardAsync(decimal initialBalance)
    {
        var newCard = new Card(GenerateUniqueCardNumber())
        {
            Balance = initialBalance
        };

        await unitOfWork.Cards.AddAsync(newCard);
        return newCard;
    }

    public async Task<Card?> GetCardByNumberAsync(string cardNumber)
    {
        return (await unitOfWork.Cards.FindAsync(x => x.CardNumber == cardNumber)).FirstOrDefault();
    }
    
    public async Task<bool> UpdateBalanceAsync(string cardNumber, decimal newBalance)
    {
        var card = await GetCardByNumberAsync(cardNumber);
        if (card == null)
        {
            return false;
        }

        card.Balance = newBalance;
        unitOfWork.Cards.Update(card);
        await unitOfWork.SaveChangesAsync();

        return true;
    }

    private string GenerateUniqueCardNumber()
    {
        var random = new Random();
        return string.Concat(Enumerable.Range(0, 15).Select(_ => random.Next(0, 10).ToString()));
    }
}