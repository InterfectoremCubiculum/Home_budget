using Home_budget_library.Models;
using Microsoft.EntityFrameworkCore;
namespace Home_budget_library.Controllers
{

    public class UserController
    {
        private readonly HomeBudgetDbContext _context;
        public UserController()
        {
            _context = new HomeBudgetDbContext();
        }
        /// <summary>
        /// Is validating login proccess 
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns>
        /// True - When the password and username is correct
        /// False - When password or username is incorrect
        /// </returns>
        public bool ValidateLogin(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                return false;
            }
            var user = _context.Users.FirstOrDefault(u => u.Name == username && u.Password == password);
            return user != null;
        }

        /// <summary>
        /// Check username if is correct to create new account
        /// </summary>
        /// <param name="username"></param>
        /// <returns>
        /// 0 - All is correct
        /// 1 - Username is too short
        /// 2 - Username is not available
        /// 3 - Username is too short and not available
        /// </returns>
        public int Create_User_Check_UserName(string username) 
        {
            int errorValue = 0;
            if (username.Length < 6)
            {
                errorValue += 1;
            }
            if(IsUser(username) == true)
            {
                errorValue += 2;
            }
            return errorValue;
        }
        /// <summary>
        /// Check password if is correct to create new account
        /// </summary>
        /// <param name="password"></param>
        /// <returns>
        /// True - It`s correct
        /// False - It`s not correct (Too short password)
        /// </returns>
        public bool Create_User_Check_Password(string password) {
            if (password.Length < 8) {
                return false;
            }
            else return true;
        }
        public bool IsUser(string username) 
        {
            bool userExists = _context.Users.Any(u => u.Name.ToLower().Equals(username.ToLower()));
            return userExists;
        }
        public bool AddUser(string username, string password) 
        {
            User user = new User();
            user.Name = username;
            user.Password = password;
            _context.Users.Add(user);
            return Save();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }
    }
}
