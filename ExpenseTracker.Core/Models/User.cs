namespace ExpenseTracker.Core.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public DateTimeOffset CreateDate { get; set; }
        public DateTimeOffset? UpdateDate { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public bool LockedOut { get; set; }
        public short LoginTries { get; set; }
        public List<Transaction> Transactions { get; set; } = new List<Transaction>();
    }
}
