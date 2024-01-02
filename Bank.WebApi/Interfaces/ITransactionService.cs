using Bank.WebApi.Model.DTOs;
using Bank.WebApi.Model.Entities;

namespace Bank.WebApi.Interfaces
{
    public interface ITransactionService
    {
        public Task CreateTransactionAsync(Transaction transaction);
        Task<IEnumerable<TransactionEntity>> GetTransactionsForUserAsync(int userId);
    }
}