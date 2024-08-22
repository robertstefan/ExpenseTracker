using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExpenseTracker.API.DTOs;
using ExpenseTracker.Core.Common.Authentication;
using ExpenseTracker.Core.Models;

namespace ExpenseTracker.API.Common.Authentication
{
    public class AuthenticationResult(AuthenticationResponse response)
    {
        public UserDTO User { get; init; } = new UserDTO(response.User);

        public string Token { get; init; } = response.Token;
    }
}