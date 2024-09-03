using ExpenseTracker.Core.Interfaces;
using ExpenseTracker.Core.Models;

namespace ExpenseTracker.Core.Services
{
  public class TransactionService
  {
    private readonly ITransactionRepository _transactionRepository;
    private readonly ICurrencyExchangeProvider _currencyExchangeProvider;

    public TransactionService(ITransactionRepository transactionRepository, ICurrencyExchangeProvider exchangeProvider)
    {
      _transactionRepository = transactionRepository;
      _currencyExchangeProvider = exchangeProvider;
    }

    public async Task<Guid> AddTransactionAsync(Transaction transaction)
    {
      return await _transactionRepository.AddTransactionAsync(transaction);
    }

    public async Task<Transaction> GetTransactionByIdAsync(Guid transactionId)
    {
      return await _transactionRepository.GetTransactionByIdAsync(transactionId);
    }

    public async Task<IEnumerable<Transaction>> GetTransactionsPaginatedAsync(int offset, int limit)
    {
      return await _transactionRepository.GetTransactionsPaginatedAsync(offset, limit);
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

    public async Task<IEnumerable<Transaction>> GetTransactionByCategoryIdAsync(Guid categoryId)
    {
      return await _transactionRepository.GetTransactionByCategoryIdAsync(categoryId);
    }
  }
}
