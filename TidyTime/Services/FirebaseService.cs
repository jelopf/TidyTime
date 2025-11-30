using Firebase.Database;
using Firebase.Database.Query;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TidyTime.Models;

namespace TidyTime.Services
{
    public class FirebaseService : IFirebaseService
    {
        private readonly FirebaseClient _firebaseClient;

        public FirebaseService()
        {
            // Указываем URL вашего проекта Firebase (Realtime Database)
            _firebaseClient = new FirebaseClient("https://tidytime-d27eb-default-rtdb.firebaseio.com/");
        }

        public async Task PostUserAsync(User user)
        {
            await _firebaseClient
                .Child("users")
                .PostAsync(user);
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            var users = await _firebaseClient
                .Child("users")
                .OnceAsync<User>();

            return users.Select(u => u.Object).ToList();
        }
    }
}
