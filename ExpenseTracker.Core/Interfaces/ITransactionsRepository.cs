using ExpenseTracker.Core.Models;

namespace ExpenseTracker.Core.Interfaces
{
    public interface ITransactionsRepository
    {
        Task<Guid> CreateTransactionAsync(Transaction transaction);

        Task<Transaction> GetTransactionByIdAndCategoryIdAsync(Guid transactionId, Guid categoryId);

        Task<Transaction> GetTransactionByIdAsync(Guid transactionId);

        Task<IEnumerable<Transaction>> GetAllTransactionsAsync();

        Task<Transaction?> UpdateTransactionAsync(Transaction transaction);

        Task<bool> DeleteTransactionAsync(Guid transactionId);

        Task<IEnumerable<Transaction>> GetTransactionsByTypeAsync(TransactionType transactionType);
    }
}
