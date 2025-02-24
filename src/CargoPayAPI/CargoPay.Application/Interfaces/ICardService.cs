using CargoPay.Domain.Entities;

namespace CargoPay.Application.Interfaces;

public interface ICardService
{
    Task<Card> CreateCardAsync(decimal initialBalance);
    Task<Card?> GetCardByNumberAsync(string cardNumber);
    Task<bool> UpdateBalanceAsync(string cardNumber, decimal newBalance);
}