using CargoPay.Application.Dtos.Cards;
using CargoPay.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CargoPay.Api.Controllers;

[ApiController]
[Authorize]
[Route("[controller]")]
public class CardController(ICardService cardService) : ControllerBase
{
    [HttpPost("create", Name = "Card")]
    public async Task<IActionResult> CreateCard(CreateCardRequest cardRequest)
    {
        return Ok(await cardService.CreateCardAsync(cardRequest));
    }
    
    [HttpGet("getBalance", Name = "GetBalance")]
    public async Task<IActionResult> GetBalance(string cardNumber)
    {
        return Ok(await cardService.GetCardByNumberAsync(cardNumber));
    }
}