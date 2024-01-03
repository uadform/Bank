using Bank.WebApi.Interfaces;
using Bank.WebApi.Model.DTOs;
using Bank.WebApi.Model.Entities;
using Bank.WebApi.Model.Entitities;
using Bank.WebApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace Bank.WebApi.Controllers
{
    [ApiController]
    [Route("Account")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAccount([FromBody] AccountCreation accountCreationDto)
        {
                await _accountService.CreateAccountAsync(accountCreationDto);
                return Ok();
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Account>>> GetAll()
        {
            var accounts = await _accountService.GetAllAccountsAsync();
            return Ok(accounts);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Account>> GetById(int id)
        {
            var account = await _accountService.GetAccountByIdAsync(id);
            if (account == null)
            {
                return NotFound();
            }

            return Ok(account);
        }

        [HttpPost("TopUp")]
        public async Task<IActionResult> TopUp([FromBody] TopUp topUpDto)
        {
            if (topUpDto.Amount <= 0)
            {
                return BadRequest("Top-up amount must be greater than zero.");
            }

            var topUpEntity = new TopUpEntity
            {
                AccountId = topUpDto.AccountId,
                Amount = topUpDto.Amount,
                Timestamp = DateTime.UtcNow
            };
            await _accountService.TopUpAccountAsync(topUpEntity);
            return Ok();
        }

    }
}
