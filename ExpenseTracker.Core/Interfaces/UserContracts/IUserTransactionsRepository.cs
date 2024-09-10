using ExpenseTracker.Core.Common.Pagination;
using ExpenseTracker.Core.Models;

namespace ExpenseTracker.Core.Interfaces.UserContracts;

public interface IUserTransactionsRepository
{
    Task<PaginatedResponse<Transaction>> GetUserTransactionAsync(Guid userId, int pageNumber, int pageSize);
    Task<PaginatedResponse<Transaction>> GetUserTransactionsByTimeRangePaginatedAsync(Guid userId, DateRange dateRange, int pageNumber, int pageSize);
}
