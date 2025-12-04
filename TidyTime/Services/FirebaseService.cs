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

        public async Task<List<User>> GetAllUsersAsync()
        {
            var users = await FirebaseClient
                .Child("users")
                .OnceAsync<User>();

            return users.Select(u => u.Object).ToList();
        }
    }
}
