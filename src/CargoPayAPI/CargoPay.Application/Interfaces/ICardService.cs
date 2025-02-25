using CargoPay.Application.Dtos.Cards;

namespace CargoPay.Application.Interfaces;

public interface ICardService
{
    Task<CreateCardResponse> CreateCardAsync(CreateCardRequest cardRequest);
    Task<CreateCardResponse?> GetCardByNumberAsync(string cardNumber);
    Task<bool> UpdateBalanceAsync(string cardNumber, decimal newBalance);
}