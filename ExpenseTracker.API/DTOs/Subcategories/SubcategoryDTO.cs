using ExpenseTracker.API.DTOs.Transactions;
using ExpenseTracker.Core.Models;

namespace ExpenseTracker.API.DTOs.Subcategories
{
    public class SubcategoryDTO
    {
        public Guid Id { get; set; }

        public string SubcategoryName { get; set; } = string.Empty;

        public List<TransactionDTO> Transactions { get; set; } = new List<TransactionDTO>();

        public SubcategoryDTO() { }

        public SubcategoryDTO(Subcategory subcategory)
        {
            Id = subcategory.Id;
            SubcategoryName = subcategory.SubcategoryName;
            Console.WriteLine(subcategory.Transactions.Count);

            foreach (var transaction in subcategory.Transactions)
            {
                Transactions.Add(new TransactionDTO(transaction));
            }

        }
    }
}
