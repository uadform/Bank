using Bank.WebApi.Interfaces;
using Bank.WebApi.Model.DTOs;
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
            if (_db.State != ConnectionState.Open)
            {
                _db.Open();
            }
            var transactionScope = _db.BeginTransaction();
            try
            {
                var decreaseBalanceQuery = "UPDATE Accounts SET Balance = Balance - @Amount WHERE AccountId = @FromAccountId";
                await _db.ExecuteAsync(decreaseBalanceQuery, new { transaction.FromAccountId, Amount = transaction.Amount + transaction.TransactionFee }, transactionScope);

                var increaseBalanceQuery = "UPDATE Accounts SET Balance = Balance + @Amount WHERE AccountId = @ToAccountId";
                await _db.ExecuteAsync(increaseBalanceQuery, new { transaction.ToAccountId, transaction.Amount }, transactionScope);

                var insertTransactionQuery = @"
                INSERT INTO Transactions (FromAccountId, ToAccountId, Amount, TransactionFee, Timestamp)
                VALUES (@FromAccountId, @ToAccountId, @Amount, @TransactionFee, @Timestamp)";
                await _db.ExecuteAsync(insertTransactionQuery, transaction, transactionScope);

                transactionScope.Commit();
            }
            catch
            {
                transactionScope.Rollback();
                throw;
            }
        }

        public async Task<IEnumerable<TransactionEntity>> GetTransactionsForUserAsync(int userId)
        {
            var query = @"
            SELECT t.TransactionId, t.FromAccountId, t.ToAccountId, t.Amount, t.TransactionFee, t.Timestamp 
            FROM Transactions t
            JOIN Accounts a ON t.FromAccountId = a.AccountId OR t.ToAccountId = a.AccountId
            WHERE a.UserId = @UserId";

            var transactions = await _db.QueryAsync<TransactionEntity>(query, new { UserId = userId });
            return transactions;
        }

    }
}
