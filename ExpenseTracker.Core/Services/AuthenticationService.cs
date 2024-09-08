using ExpenseTracker.Core.Interfaces;
using ExpenseTracker.Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Core.Services
{
    public class AuthenticationService
    {
        private readonly IUsersRepository _usersRepository;
        private readonly PasswordHasher<User> _passwordHasher;
        private readonly IConfiguration _configuration;
        public AuthenticationService (IConfiguration configuration, IUsersRepository usersRepository)
        {
            _passwordHasher = new PasswordHasher<User>();
            _configuration = configuration;
            _usersRepository = usersRepository;

        }

        public async Task<string> Login (User user)
        {
            var userToLogin = await _usersRepository.GetUserByUsernameAsync(user.Username);

            bool isValidPassword = userToLogin.UserPassword == _passwordHasher.HashPassword(user, user.UserPassword);

            //if (isValidPassword == false || userToLogin == null)
            //{
            //    return null;
            //}

            var token = await GenerateToken(user);

            return token;
        }

        public async Task<int> Register (User user)
        {
            var hashedPassword = _passwordHasher.HashPassword(user, user.UserPassword);
            user.UserPassword = hashedPassword;
            return await _usersRepository.CreateUserAsync(user);
        }

        private async Task<string> GenerateToken (User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]));

            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub,user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email,user.Email),
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["JwtSettings:Issuer"],
                audience: _configuration["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToInt32(_configuration["JwtSettings:DurationInMinutes"])),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
