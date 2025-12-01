using System.Threading.Tasks;
using TidyTime.Models;

namespace TidyTime.Services;

public interface IAuthService
{
    Task<bool> RegisterUserAsync(User user);
    Task<User?> LoginUserAsync(string login, string password);
    Task<bool> CheckIfUserExistsAsync(string login);
}
