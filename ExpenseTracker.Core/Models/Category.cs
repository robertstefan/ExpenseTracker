namespace ExpenseTracker.Core.Models
{
    public class Category
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string CategoryName { get; set; } = string.Empty;

        public List<Transaction> Transactions { get; set; } = new List<Transaction>();
    }
}
