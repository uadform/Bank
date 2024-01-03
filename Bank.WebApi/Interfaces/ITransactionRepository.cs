using Bank.WebApi.Model.DTOs;
using Bank.WebApi.Model.Entities;

namespace Bank.WebApi.Interfaces
{
    public interface ITransactionRepository
    {
        Task CreateTransactionAsync(TransactionEntity transaction);
        public Task<IEnumerable<TransactionEntity>> GetTransactionsForUserAsync(int userId);
    }
}