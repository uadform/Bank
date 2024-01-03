using Bank.WebApi.Exceptions;
using Bank.WebApi.Interfaces;
using Bank.WebApi.Model.DTOs;
using Bank.WebApi.Model.Entities;
using Bank.WebApi.Model.Entitities;
using Bank.WebApi.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Bank.WebApi.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IUserRepository _userRepository;

        public AccountService(IAccountRepository accountRepository, IUserRepository userRepository)
        {
            _accountRepository = accountRepository;
            _userRepository = userRepository;
        }

        private void ValidateAccountType(string type)
        {
            var validTypes = new List<string> { "Savings", "Default" };
            if (!validTypes.Contains(type))
            {
                throw new InvalidOperationException($"Invalid account type. Allowed types are: {string.Join(", ", validTypes)}.");
            }
        }

        public async Task CreateAccountAsync(AccountCreation account)
        {
            ValidateAccountType(account.Type);
            if (account.Balance < 0)
            {
                throw new InvalidOperationException("Initial balance cannot be negative.");
            }
            var userExists = await _userRepository.UserExists(account.UserId);
            if (!userExists)
            {
                throw new UserNotFoundException($"User with ID {account.UserId} does not exist.");
            }
            var accountCount = await _accountRepository.GetAccountCountForUserAsync(account.UserId);
            if (accountCount >= 2)
            {
                throw new InvalidOperationException("User cannot have more than two accounts.");
            }
            var accountEntity = new AccountEntity
            {
                UserId = account.UserId,
                Type = account.Type,
                Balance = account.Balance
            };


            await _accountRepository.CreateAccountAsync(accountEntity);
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
