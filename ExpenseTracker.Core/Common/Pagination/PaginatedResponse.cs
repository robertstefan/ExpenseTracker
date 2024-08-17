using ExpenseTracker.Core.Interfaces.Common;

namespace ExpenseTracker.Core.Common.Pagination;

public class PaginatedResponse<T> where T : IEntity
{
    public int TotalCount { get; set; }
    public ICollection<T> Rows { get; set; }
}
