using ExpenseTracker.Core.Interfaces;
using ExpenseTracker.Core.Models;

namespace ExpenseTracker.Core.Services
{
    public class TransactionsService
    {
        private readonly ITransactionsRepository _transactionRepository;

        public TransactionsService(ITransactionsRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }

        public async Task<Transaction> GetTransactionByIdAsync(Guid transactionId)
        {
            return await _transactionRepository.GetTransactionByIdAsync(transactionId);
        }

        public async Task<IEnumerable<Transaction>> GetAllTransactionsAsync()
        {
            return await _transactionRepository.GetAllTransactionsAsync();
        }

        public async Task<IEnumerable<Transaction>> GetTransactionsByTypeAsync(TransactionType transactionType)
        {
            return await _transactionRepository.GetTransactionsByTypeAsync(transactionType);
        }

        public async Task<IEnumerable<Transaction>> GetTransactionsByCategoryIdAsync(Guid categoryId)
        {
            return await _transactionRepository.GetTransactionsByCategoryIdAsync(categoryId);
        }

        public async Task<Guid> AddTransactionAsync(Transaction transaction)
        {
            return await _transactionRepository.CreateTransactionAsync(transaction);
        }

        public async Task<Transaction?> UpdateTransactionAsync(Transaction transaction)
        {
            return await _transactionRepository.UpdateTransactionAsync(transaction);
        }

        public async Task<bool> DeleteTransactionAsync(Guid transactionId)
        {
            return await _transactionRepository.DeleteTransactionAsync(transactionId);
        }
    }
}
