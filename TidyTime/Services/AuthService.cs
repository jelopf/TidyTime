using System.Linq;
using System.Threading.Tasks;
using TidyTime.Models;

namespace TidyTime.Services
{
    public class AuthService : IAuthService
    {
        private readonly IFirebaseService _firebaseService;

        public AuthService(IFirebaseService firebaseService)
        {
            _firebaseService = firebaseService;
        }

        public async Task<bool> RegisterUserAsync(User user)
        {
            // Проверяем, существует ли уже пользователь
            if(await CheckIfUserExistsAsync(user.Login))
                return false;

            await _firebaseService.PostUserAsync(user);
            return true;
        }

        public async Task<User?> LoginUserAsync(string login, string password)
        {
            var users = await _firebaseService.GetAllUsersAsync();
            return users.FirstOrDefault(u => u.Login == login && u.PasswordHash == password);
        }

        public async Task<bool> CheckIfUserExistsAsync(string login)
        {
            var users = await _firebaseService.GetAllUsersAsync();
            return users.Any(u => u.Login == login);
        }
    }
}
