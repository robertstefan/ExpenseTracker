using ExpenseTracker.Core.Interfaces;
using ExpenseTracker.Core.Models;

namespace ExpenseTracker.Core.Services
{
  public class UserService
  {
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepo)
    {
      _userRepository = userRepo;
    }

    public async Task<User?> GetUserByEmail(string email)
    {
      return await _userRepository.GetUserByEmail(email);
    }

    public async Task<User?> GetUserById(int userId)
    {
      return await _userRepository.GetUserById(userId);
    }

    public async Task<bool> RemoveUser(int userId)
    {
      try
      {
        var res = await _userRepository.RemoveUserAsync(userId);

        return res;
      }
      catch (Exception ex)
      {
        throw;
      }
    }

    public async Task<User> UpdateUserAsync(User user)
    {
      return await _userRepository.UpdateUserAsync(user);
    }

    public async Task<IEnumerable<User>> GetAllUsersAsync()
    {
      return await _userRepository.GetAllUsersAsync();
    }
  }
}
