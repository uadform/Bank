using Bank.WebApi.Interfaces;
using Bank.WebApi.Model.DTOs;
using Bank.WebApi.Model.Entities;

namespace Bank.WebApi.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _transactionsRepository;
        private readonly IAccountRepository _accountRepository;

        public TransactionService(ITransactionRepository transactionsRepository, IAccountRepository accountRepository)
        {
            _transactionsRepository = transactionsRepository;
            _accountRepository = accountRepository;
        }

        public async Task CreateTransactionAsync(Transaction transaction)
        {
            var transactionEntity = new TransactionEntity
            {
                FromAccountId = transaction.FromAccountId,
                ToAccountId = transaction.ToAccountId,
                Amount = transaction.Amount,
            };
            var senderAccount = await _accountRepository.GetAccountByIdAsync(transaction.FromAccountId);
            if (senderAccount.Balance < transaction.Amount + transactionEntity.TransactionFee)
            {
                throw new InvalidOperationException("Insufficient funds to complete the transaction.");
            }

            await _accountRepository.DecreaseBalanceAsync(transactionEntity.FromAccountId, transactionEntity.Amount + transactionEntity.TransactionFee);
            await _accountRepository.IncreaseBalanceAsync(transactionEntity.ToAccountId, transactionEntity.Amount);

            transactionEntity.Timestamp = DateTime.UtcNow;
            await _transactionsRepository.CreateTransactionAsync(transactionEntity);
        }

        public async Task<IEnumerable<TransactionEntity>> GetTransactionsForUserAsync(int userId)
        {
            return await _transactionsRepository.GetTransactionsForUserAsync(userId);
        }

    }
}
