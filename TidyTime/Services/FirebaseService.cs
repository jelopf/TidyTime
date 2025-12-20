using Firebase.Database;
using Firebase.Database.Query;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TidyTime.Models;

namespace TidyTime.Services
{
    public class FirebaseService :  BaseFirebaseService, IFirebaseService
    {
        public async Task PostUserAsync(User user)
        {
            await FirebaseClient
                .Child("users")
                .PostAsync(user);
        }

        public async Task UpdateUserAsync(User user)
        {
            if (string.IsNullOrEmpty(user.Id))
                return;
                
            await FirebaseClient
                .Child("users")
                .Child(user.Id)
                .PutAsync(user);
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            var users = await FirebaseClient
                .Child("users")
                .OnceAsync<User>();

            return users
                .Select(u =>
                {
                    var user = u.Object;
                    user.Id = u.Key;
                    return user;
                })
                .ToList();
        }

        public async Task<User?> GetUserByIdAsync(string userId)
        {
            var user = await FirebaseClient
                .Child("users")
                .Child(userId)
                .OnceSingleAsync<User>();
                
            if (user != null)
            {
                user.Id = userId;
            }
            
            return user;
        }
    }
}
