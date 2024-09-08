using ExpenseTracker.API.DTOs.Transactions;
using ExpenseTracker.Core.Models;

namespace ExpenseTracker.API.DTOs.Users
{
    public class UserDTO
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string UserPassword { get; set; }
        public DateTimeOffset CreateDate { get; set; }
        public DateTimeOffset? UpdateDate { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public bool LockedOut { get; set; }
        public short LoginTries { get; set; }
        public List<TransactionDTO> Transactions { get; set; } = new List<TransactionDTO>();

        public UserDTO ()
        {

        }

        public UserDTO (User user)
        {
            Username = user.Username;
            Email = user.Email;
            UserPassword = user.UserPassword;
            CreateDate = user.CreateDate;
            UpdateDate = user.UpdateDate;
            LastName = user.LastName;
            FirstName = user.FirstName;
            LockedOut = user.LockedOut;
            LoginTries = user.LoginTries;
            foreach (var transaction in user.Transactions)
            {
                Transactions.Add(new TransactionDTO(transaction));
            }
        }
    }
}
