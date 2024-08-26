using ExpenseTracker.Core.Interfaces.Common;

namespace ExpenseTracker.Core.Models
{
    public class User : IEntity
    {
        public Guid Id { get; private set; }
        public string Username { get; private set; }
        public string Email { get; private set; }
        public string PasswordHash { get; private set; }

        public string LastName { get; private set; }
        public string FirstName { get; private set; }

        public bool LockedOut { get; private set; }
        public short LoginTries { get; private set; }

        public bool IsDeleted { get; private set; }

        public DateTimeOffset? CreatedDateTime { get; private set; }
        public DateTimeOffset? UpdatedDateTime { get; private set; }

        private User()
        {

        }
        private User(
            Guid id,
            string username,
            string email,
            string passwordHash,
            string lastName,
            string firstName,
            bool lockedOut,
            short loginTries,
            DateTimeOffset? createdDateTime = null,
            DateTimeOffset? updatedDateTime = null)
        {
            Id = id;
            Username = username;
            Email = email;
            PasswordHash = passwordHash;
            LastName = lastName;
            FirstName = firstName;
            LockedOut = lockedOut;
            LoginTries = loginTries;
            CreatedDateTime = createdDateTime;
            UpdatedDateTime = updatedDateTime;
        }

        public static User CreateNew(
            string username,
            string email,
            string passwordHash,
            string lastName,
            string firstName)
        {
            return new(
                Guid.NewGuid(),
                username,
                email,
                passwordHash,
                lastName,
                firstName,
                false,
                0
            );
        }
        public static User Create(
            Guid id,
            string username,
            string email,
            string passwordHash,
            string lastName,
            string firstName,
            bool lockedOut,
            short loginTries,
            DateTimeOffset? createdDateTime = null,
            DateTimeOffset? updatedDateTime = null)
        {
            return new(
                id,
                username,
                email,
                passwordHash,
                lastName,
                firstName,
                lockedOut,
                loginTries,
                createdDateTime,
                updatedDateTime
            );
        }
    }
}