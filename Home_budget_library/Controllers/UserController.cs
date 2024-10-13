using Home_budget_library.Models;
namespace Home_budget_library.Controllers
{

    public class UserController
    {
        private readonly List<User> _users;

        public UserController()
        {
            // Initialize with a sample user for testing
            _users = new List<User>
            {
                new User { Id = 1, Name = "admin", Password = "password" }
            };
        }

        public bool ValidateLogin(string username, string password)
        {
            var user = _users.Find(u => u.Name == username && u.Password == password);
            return user != null;
        }
    }
}
