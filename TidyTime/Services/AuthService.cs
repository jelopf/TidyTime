using System.Linq;
using System.Threading.Tasks;
using TidyTime.Models;

namespace TidyTime.Services
{
    public class AuthService : IAuthService
    {
        private readonly IFirebaseService _firebaseService;
        private User? _currentUser;

        public AuthService(IFirebaseService firebaseService)
        {
            _firebaseService = firebaseService;
        }

        public async Task<bool> RegisterUserAsync(User user, string password)
        {
            if (await CheckIfUserExistsAsync(user.Login))
                return false;

            user.SetPassword(password);
            await _firebaseService.PostUserAsync(user);
            return true;
        }

        public async Task<User?> LoginUserAsync(string login, string password)
        {
            var users = await _firebaseService.GetAllUsersAsync();
            var user = users.FirstOrDefault(u => u.Login == login);
            
            if (user == null) 
                return null;
            
            if (user.VerifyPassword(password))
            {
                _currentUser = user;
                return user;
            }
            
            return null;
        }

        public async Task<bool> CheckIfUserExistsAsync(string login)
        {
            var users = await _firebaseService.GetAllUsersAsync();
            return users.Any(u => u.Login == login);
        }

        public User? GetCurrentUser()
        {
            return _currentUser;
        }
    }
}
