using Bank.WebApi.Interfaces;
using Bank.WebApi.Model.DTOs;
using Bank.WebApi.Model.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Bank.WebApi.Controllers
{
    [ApiController]
    [Route("Transaction")]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _transactionsService;

        public TransactionController(ITransactionService transactionsService)
        {
            _transactionsService = transactionsService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateTransaction([FromBody] Transaction transaction)
        {
            try
            {
                await _transactionsService.CreateTransactionAsync(transaction);
                return Ok();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("User/{userId}")]
        public async Task<IActionResult> GetTransactionsForUser(int userId)
        {
            var transactions = await _transactionsService.GetTransactionsForUserAsync(userId);
            return Ok(transactions);
        }
    }
}
