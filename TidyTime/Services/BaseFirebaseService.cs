using Firebase.Database;

namespace TidyTime.Services
{
    public abstract class BaseFirebaseService
    {
        protected readonly FirebaseClient FirebaseClient;
        
        protected BaseFirebaseService()
        {
            FirebaseClient = new FirebaseClient("https://tidytime-d27eb-default-rtdb.firebaseio.com/");
        }
    }
}