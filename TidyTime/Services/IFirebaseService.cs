using System.Collections.Generic;
using System.Threading.Tasks;
using TidyTime.Models;

namespace TidyTime.Services
{
    public interface IFirebaseService
    {
        Task PostUserAsync(User user);
        Task<List<User>> GetAllUsersAsync();
    }
}
