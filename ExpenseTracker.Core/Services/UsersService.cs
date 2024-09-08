using ExpenseTracker.Core.Interfaces;
using ExpenseTracker.Core.Models;

namespace ExpenseTracker.Core.Services
{
    public class UsersService
    {
        private readonly IUsersRepository _usersRepository;

        public UsersService(IUsersRepository usersRepository)
        {
            _usersRepository = usersRepository;
        }

        public async Task<IEnumerable<User>> GetUsers()
        {
            return await _usersRepository.GetUsers();
        }

        public async Task<User> GetUserByIdAsync(int userId)
        {
            return await _usersRepository.GetUserByIdAsync(userId);
        }
        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _usersRepository.GetUserByEmailAsync(email);
        }
        public async Task<User> GetUserByUsernameAsync(string username)
        {
            return await _usersRepository.GetUserByUsernameAsync(username);
        }
        public async Task<User> UpdateUserAsync(User user)
        {
            return await _usersRepository.UpdateUserAsync(user);
        }
        public async Task<bool> DeleteUserAsync(int userId)
        {
            try
            {
                var res = await _usersRepository.DeleteUserAsync(userId);

                return res;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
    }
}
