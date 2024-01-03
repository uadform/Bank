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
        public Task<bool> UserExists(int userId);
        public Task<AccountEntity> GetAccountByTypeAndUserId(string type, int userId);

    }
}   