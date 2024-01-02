using Bank.WebApi.Model.Entities;

namespace Bank.WebApi.Interfaces
{
    public interface ITransactionRepository
    {
        Task CreateTransactionAsync(TransactionEntity transaction);
        Task<IEnumerable<TransactionEntity>> GetTransactionsForUserAsync(int userId);
    }
}