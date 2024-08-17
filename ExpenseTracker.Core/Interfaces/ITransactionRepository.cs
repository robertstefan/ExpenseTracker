using ExpenseTracker.Core.Common.Pagination;
using ExpenseTracker.Core.Models;

namespace ExpenseTracker.Core.Interfaces;

public interface ITransactionRepository
{
  Task<Guid> AddTransactionAsync(Transaction transaction);

  Task<Transaction> GetTransactionByIdAsync(Guid transactionId);

  Task<PaginatedResponse<Transaction>?> GetTransactionsPaginatedAsync(int PageNumber, int PageSize);

  Task<bool> UpdateTransactionAsync(Transaction transaction);

  Task<bool> DeleteTransactionAsync(Guid transactionId, bool SoftDelete);

  Task<IEnumerable<Transaction>> GetTransactionsByTypeAsync(int transactionType);
}

