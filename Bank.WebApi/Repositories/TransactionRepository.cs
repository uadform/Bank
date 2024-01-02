using Bank.WebApi.Interfaces;
using Bank.WebApi.Model.Entities;
using Dapper;
using System.Data;

namespace Bank.WebApi.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly IDbConnection _db;
        public TransactionRepository(IDbConnection db)
        {
            _db = db;
        }
        public async Task CreateTransactionAsync(TransactionEntity transaction)
        {
            var query = @"
            INSERT INTO Transactions (FromAccountId, ToAccountId, Amount, TransactionFee, Timestamp)
            VALUES (@FromAccountId, @ToAccountId, @Amount, @TransactionFee, @Timestamp)";

            await _db.ExecuteAsync(query, transaction);
        }

        public async Task<IEnumerable<TransactionEntity>> GetTransactionsForUserAsync(int userId)
        {
            var query = @"
        SELECT t.* 
        FROM Transactions t
        WHERE t.FromAccountId IN (SELECT AccountId FROM Accounts WHERE UserId = @UserId)
           OR t.ToAccountId IN (SELECT AccountId FROM Accounts WHERE UserId = @UserId)";

            return await _db.QueryAsync<TransactionEntity>(query, new { UserId = userId });
        }

    }
}
