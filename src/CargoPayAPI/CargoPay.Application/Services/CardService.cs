using CargoPay.Application.Dtos.Cards;
using CargoPay.Application.Interfaces;
using CargoPay.Application.Interfaces.Infrastructure.Persistence.Repositories;
using CargoPay.Domain.Entities;
using CargoPay.Domain.Enums;

namespace CargoPay.Application.Services;

public class CardService(IUnitOfWork unitOfWork, IUserService userService) : ICardService
{
    public async Task<CreateCardResponse> CreateCardAsync(CreateCardRequest cardRequest)
    {
        var authenticatedUser = await userService.Authenticated();
        if (authenticatedUser.Role != Role.Admin)
        {
            throw new Exception("Unauthorized.");
        }

        var user = (await unitOfWork.Users.FindAsync(x => x.Username == cardRequest.Username)).FirstOrDefault();
        if (user == null)
        {
            throw new Exception("User not found");
        }

        var newCard = new Card(user.Id, GenerateUniqueCardNumber())
        {
            Balance = cardRequest.AuthorizedAmount
        };

        await unitOfWork.Cards.AddAsync(newCard);
        await unitOfWork.SaveChangesAsync();

        return new CreateCardResponse
        {
            Username = user.Username,
            CardNumber = newCard.CardNumber,
            Balance = newCard.Balance
        };
    }

    public async Task<CreateCardResponse?> GetCardByNumberAsync(string cardNumber)
    {
        var authenticatedUser = await userService.Authenticated();

        var card = (await unitOfWork.Cards.FindAsync(x => x.CardNumber == cardNumber &&
                                                          (x.UserId == authenticatedUser.Id ||
                                                           authenticatedUser.Role == Role.Admin))).FirstOrDefault();

        if (card is null)
        {
            throw new Exception("Card not found");
        }

        var user = await unitOfWork.Users.GetByIdAsync(card.UserId);
        return new CreateCardResponse
        {
            Username = user.Username,
            CardNumber = card.CardNumber,
            Balance = card.Balance
        };
    }

    public async Task<bool> UpdateBalanceAsync(string cardNumber, decimal newBalance)
    {
        var authenticatedUser = await userService.Authenticated();

        var card = (await unitOfWork.Cards.FindAsync(
            x => x.CardNumber == cardNumber && authenticatedUser.Id == x.UserId)).FirstOrDefault();

        if (card == null)
        {
            throw new Exception("Unauthorized.");
        }

        card.Balance = newBalance;

        unitOfWork.Cards.Update(card);
        await unitOfWork.SaveChangesAsync();

        return true;
    }

    private static string GenerateUniqueCardNumber()
    {
        var random = new Random();
        return string.Concat(Enumerable.Range(0, 15).Select(_ => random.Next(0, 10).ToString()));
    }
}