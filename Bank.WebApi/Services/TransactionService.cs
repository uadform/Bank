using Bank.WebApi.Exceptions;
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

        public async Task CreateTransactionAsync(Transaction transactionDto)
        {
            var senderAccount = await _accountRepository.GetAccountByIdAsync(transactionDto.FromAccountId);
            if (senderAccount == null)
                throw new UserNotFoundException("Sender account was not found");

            if (transactionDto.Amount <= 0) 
                throw new InsufficientFundsException("Please enter amount greater than 0");

            if (transactionDto.FromAccountId == transactionDto.ToAccountId)
                throw new InvalidOperationException("Cannot transfer to the same account.");

            if (senderAccount.Balance < transactionDto.Amount + 1.00M) 
                throw new InsufficientFundsException("Insufficient funds to complete the transaction.");

            var receiverAccount = await _accountRepository.GetAccountByIdAsync(transactionDto.ToAccountId);
            if (receiverAccount == null)
                throw new UserNotFoundException("Receiver account not found.");

            var transactionEntity = new TransactionEntity
            {
                FromAccountId = transactionDto.FromAccountId,
                ToAccountId = transactionDto.ToAccountId,
                Amount = transactionDto.Amount,
                Timestamp = DateTime.UtcNow
            };

            await _transactionsRepository.CreateTransactionAsync(transactionEntity);
        }

        public async Task<IEnumerable<TransactionEntity>> GetTransactionsForUserAsync(int userId)
        {
            var userExists = await _accountRepository.UserExists(userId);
            if (!userExists) throw new UserNotFoundException("User was not found");

            return await _transactionsRepository.GetTransactionsForUserAsync(userId);
        }
    }
}
