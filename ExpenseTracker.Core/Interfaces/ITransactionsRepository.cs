using ExpenseTracker.Core.Models;

namespace ExpenseTracker.Core.Interfaces
{
    public interface ITransactionsRepository
    {
        Task<IEnumerable<Transaction>> GetTransactionsByTypeAsync(TransactionType transactionType);
        Task<IEnumerable<Transaction>> GetAllTransactionsAsync();
        Task<IEnumerable<Transaction>> GetTransactionsByCategoryIdAsync(Guid categoryId);
        Task<IEnumerable<Transaction>> GetTransactionsByUserIdAsync(int userId);
        Task<Transaction> GetTransactionByIdAsync(Guid transactionId);
        Task<Guid> CreateTransactionAsync(Transaction transaction);
        Task<Transaction?> UpdateTransactionAsync(Transaction transaction);
        Task<bool> DeleteTransactionAsync(Guid transactionId);
    }
}
