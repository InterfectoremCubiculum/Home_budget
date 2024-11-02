using Home_budget_library.Models;
namespace Home_budget_library.Controllers
{

    public class UserController
    {
        private readonly HomeBudgetDbContext _context;
        public UserController()
        {
            _context = new HomeBudgetDbContext();
            _context.Users.Any();
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
                return false;

            return _context.Users
                .AsEnumerable() // Zapytanie zostanie wykonane w pamięci
                .Any(u => u.Name.Equals(username, StringComparison.Ordinal) &&
                  u.Password.Equals(password, StringComparison.Ordinal));
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
                errorValue += 1;

            if (IsUser(username) == true)
                errorValue += 2;

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
        public bool Create_User_Check_Password(string password)
        {
            return password.Length >= 8;
        }
        public bool IsUser(string username)
        {
            return _context.Users.Any(u => u.Name.ToLower().Equals(username.ToLower()));
        }
        public bool AddUser(string username, string password)
        {
            var user = new User
            {
                Name = username,
                Password = password
            };
            _context.Users.Add(user);
            return Save();
        }

        public bool Save()
        {
            return _context.SaveChanges() > 0;
        }

        public int GetUserID(string username)
        {
            var user = _context.Users.First(u => u.Name.ToLower() == username.ToLower());
            return user?.Id ?? 0; // return 0 if user not found
        }
    }
}
