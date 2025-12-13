using System;
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
            await Task.Delay(1500);

            await CreateWelcomeTaskAsync(user);

            return true;
        }

        private async Task CreateWelcomeTaskAsync(User user)
        {
            var taskService = new TaskService();
            
            var welcomeTask = new TaskItem
            {
                Title = "Добро пожаловать!👋",
                Description = "Это ваша первая задача!",
                StartTime = DateTime.Today.AddHours(10),
                EndTime = DateTime.Today.AddHours(11),
                IsAllDay = false,
                Difficulty = 1,
                Status = Models.TaskStatus.Pending,
                OwnerId = user.Id,
                AssignedChildId = user.Id,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            
            await taskService.AddTaskAsync(welcomeTask);
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
