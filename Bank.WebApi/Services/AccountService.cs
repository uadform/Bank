using Bank.WebApi.Interfaces;
using Bank.WebApi.Model.DTOs;
using Bank.WebApi.Model.Entities;
using Bank.WebApi.Model.Entitities;
using Microsoft.AspNetCore.Mvc;

namespace Bank.WebApi.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;

        public AccountService(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public async Task CreateAccountAsync(AccountEntity account)
        {
            var accountCount = await _accountRepository.GetAccountCountForUserAsync(account.UserId);
            if (accountCount >= 2)
            {
                throw new InvalidOperationException("User cannot have more than two accounts.");
            }

            await _accountRepository.CreateAccountAsync(account);
        }

        public async Task<IEnumerable<Account>> GetAllAccountsAsync()
        {
            var accountEntities = await _accountRepository.GetAllAccountsAsync();
            return accountEntities.Select(a => new Account
            {
                AccountId = a.AccountId,
                UserId = a.UserId,
                Type = a.Type,
                Balance = a.Balance,
                User = a.User != null ? new User
                {
                    UserId = a.User.UserId,
                    Name = a.User.Name,
                    Address = a.User.Address
                } : null!
            });
        }

        public async Task<Account> GetAccountByIdAsync(int accountId)
        {
            var accountEntity = await _accountRepository.GetAccountByIdAsync(accountId);
            if (accountEntity == null) return null!;

            return new Account
            {
                AccountId = accountEntity.AccountId,
                UserId = accountEntity.UserId,
                Type = accountEntity.Type,
                Balance = accountEntity.Balance,
                User = accountEntity.User != null ? new User
                {
                    UserId = accountEntity.User.UserId,
                    Name = accountEntity.User.Name,
                    Address = accountEntity.User.Address
                } : null!
            };
        }

        public async Task TopUpAccountAsync(TopUpEntity topUpEntity)
        {
            await _accountRepository.TopUpAccountAsync(topUpEntity);
        }
    }
}
