using Bank.WebApi.Model.Entities;
using Bank.WebApi.Model.Entitities;

namespace Bank.WebApi.Interfaces
{
    public interface IAccountRepository
    {
        public Task CreateAccountAsync(AccountEntity account);
        public Task<int> GetAccountCountForUserAsync(int userId);
        public Task<IEnumerable<AccountEntity>> GetAllAccountsAsync();
        public Task<AccountEntity> GetAccountByIdAsync(int accountId);
        public Task TopUpAccountAsync(TopUpEntity topUpEntity);
        public Task DecreaseBalanceAsync(int accountId, decimal amount);
        public Task IncreaseBalanceAsync(int accountId, decimal amount);

    }
}