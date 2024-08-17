using ExpenseTracker.Core.Common.Pagination;
using ExpenseTracker.Core.Interfaces;
using ExpenseTracker.Core.Models;

namespace ExpenseTracker.Core.Services
{
  public class TransactionService
  {
    private readonly ITransactionRepository _transactionRepository;

    public TransactionService(ITransactionRepository transactionRepository)
    {
      _transactionRepository = transactionRepository;
    }

    public async Task<Guid> AddTransactionAsync(Transaction transaction)
    {
      return await _transactionRepository.AddTransactionAsync(transaction);
    }

    public async Task<Transaction> GetTransactionByIdAsync(Guid transactionId)
    {
      return await _transactionRepository.GetTransactionByIdAsync(transactionId);
    }

    public async Task<PaginatedResponse<Transaction>?> GetTransactionsPaginatedAsync(int PageNumber, int PageSize)
    {
      return await _transactionRepository.GetTransactionsPaginatedAsync(PageNumber, PageSize);
    }

    public async Task<bool> UpdateTransactionAsync(Transaction transaction)
    {
      return await _transactionRepository.UpdateTransactionAsync(transaction);
    }

    public async Task<bool> DeleteTransactionAsync(Guid transactionId, bool SoftDelete)
    {
      return await _transactionRepository.DeleteTransactionAsync(transactionId, SoftDelete);
    }

    public async Task<IEnumerable<Transaction>> GetTransactionsByTypeAsync(int transactionType)
    {
      return await _transactionRepository.GetTransactionsByTypeAsync(transactionType);
    }
  }
}
