using Bank.WebApi.Model.DTOs;
using Bank.WebApi.Model.Entities;
using Bank.WebApi.Model.Entitities;

namespace Bank.WebApi.Interfaces
{
    public interface IAccountService
    {
        public Task CreateAccountAsync(AccountEntity account);
        public Task<IEnumerable<Account>> GetAllAccountsAsync();
        public Task<Account> GetAccountByIdAsync(int accountId);
        public Task TopUpAccountAsync(TopUpEntity topUpEntity);
    }
}