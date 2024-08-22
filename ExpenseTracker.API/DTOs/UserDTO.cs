using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExpenseTracker.API.Common.Interfaces;
using ExpenseTracker.Core.Models;

namespace ExpenseTracker.API.DTOs
{
    public class UserDTO : IEntityDTO
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public DateTimeOffset? CreatedDateTime { get; set; }
        public DateTimeOffset? UpdatedDateTime { get; set; }

        public UserDTO()
        {

        }

        public UserDTO(User user)
        {
            Id = user.Id;
            Username = user.Username;
            Email = user.Email;
            LastName = user.LastName;
            FirstName = user.FirstName;
            CreatedDateTime = user.CreatedDateTime;
            UpdatedDateTime = user.UpdatedDateTime;
        }
    }
}