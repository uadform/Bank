using Bank.WebApi.Interfaces;
using Bank.WebApi.Model.Entities;
using Bank.WebApi.Model.Entitities;
using Dapper;
using System.Data;

namespace Bank.WebApi.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly IDbConnection _db;

        public AccountRepository(IDbConnection db)
        {
            _db = db;
        }

        public async Task CreateAccountAsync(AccountEntity account)
        {
            var query = "INSERT INTO Accounts (UserId, Type, Balance) VALUES (@UserId, @Type, @Balance)";
            await _db.ExecuteAsync(query, account);
        }

        public async Task<int> GetAccountCountForUserAsync(int userId)
        {
            var query = "SELECT COUNT(*) FROM Accounts WHERE UserId = @UserId";
            return await _db.ExecuteScalarAsync<int>(query, new { UserId = userId });
        }

        public async Task<IEnumerable<AccountEntity>> GetAllAccountsAsync()
        {
            var query = @"
            SELECT a.*, u.* 
            FROM Accounts a
            LEFT JOIN Users u ON a.UserId = u.UserId";

            var accounts = new List<AccountEntity>();
            await _db.QueryAsync<AccountEntity, UserEntity, AccountEntity>(
                query,
                (account, user) =>
                {
                    account.User = user;
                    accounts.Add(account);
                    return account;
                },
                splitOn: "UserId");

            return accounts;
        }

        public async Task<AccountEntity> GetAccountByIdAsync(int accountId)
        {
            var query = @"
            SELECT a.*, u.* 
            FROM Accounts a
            LEFT JOIN Users u ON a.UserId = u.UserId
            WHERE a.AccountId = @AccountId";

            var result = await _db.QueryAsync<AccountEntity, UserEntity, AccountEntity>(
                query,
                (account, user) =>
                {
                    account.User = user;
                    return account;
                },
                new { AccountId = accountId },
                splitOn: "UserId"
            );

            return result.FirstOrDefault()!;
        }

        public async Task TopUpAccountAsync(TopUpEntity topUpEntity)
        {
            var updateAccountQuery = "UPDATE Accounts SET Balance = Balance + @Amount WHERE AccountId = @AccountId";
            await _db.ExecuteAsync(updateAccountQuery, new { AccountId = topUpEntity.AccountId, Amount = topUpEntity.Amount });

            var insertTopUpQuery = "INSERT INTO TopUps (AccountId, Amount) VALUES (@AccountId, @Amount)";
            await _db.ExecuteAsync(insertTopUpQuery, new { AccountId = topUpEntity.AccountId, Amount = topUpEntity.Amount });
        }

        public async Task DecreaseBalanceAsync(int accountId, decimal amount)
        {
            var query = @"
            UPDATE Accounts 
            SET Balance = Balance - @Amount 
            WHERE AccountId = @AccountId AND Balance >= @Amount";

            var rowsAffected = await _db.ExecuteAsync(query, new { AccountId = accountId, Amount = amount });

            if (rowsAffected == 0)
            {
                throw new InvalidOperationException("Unable to decrease balance. Insufficient funds or account not found.");
            }
        }
        public async Task IncreaseBalanceAsync(int accountId, decimal amount)
        {
            var query = @"
            UPDATE Accounts 
            SET Balance = Balance + @Amount 
            WHERE AccountId = @AccountId";

            var rowsAffected = await _db.ExecuteAsync(query, new { AccountId = accountId, Amount = amount });

            if (rowsAffected == 0)
            {
                throw new InvalidOperationException("Unable to increase balance. Account not found.");
            }
        }
    }
}
