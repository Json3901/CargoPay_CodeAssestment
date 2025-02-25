using CargoPay.Application.Dtos.Transactions;
using CargoPay.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CargoPay.Api.Controllers;

[ApiController]
[Authorize]
public class PaymentController(IPaymentService paymentService) : ControllerBase
{
    [HttpPost("pay", Name = "Pay")]
    public async Task<IActionResult> Pay(TransactionRequest transaction)
    {
        return Ok(await paymentService.ProcessPayment(transaction));
    }
}