namespace ExpenseTracker.Core.Models
{
    public class Subcategory
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string SubcategoryName { get; set; } = string.Empty;

        public Guid CategoryId { get; set; }

        public List<Transaction> Transactions { get; set; } = new List<Transaction>();
    }
}
