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

    public async Task<IEnumerable<Transaction>> GetAllTransactionsAsync()
    {
      return await _transactionRepository.GetAllTransactionsAsync();
    }

    public async Task<Transaction?> UpdateTransactionAsync(Transaction transaction)
    {
      return await _transactionRepository.UpdateTransactionAsync(transaction);
    }

    public async Task<bool> DeleteTransactionAsync(Guid transactionId)
    {
      return await _transactionRepository.DeleteTransactionAsync(transactionId);
    }

    public async Task<IEnumerable<Transaction>> GetTransactionsByTypeAsync(int transactionType)
    {
      return await _transactionRepository.GetTransactionsByTypeAsync(transactionType);
    }
  }
}
