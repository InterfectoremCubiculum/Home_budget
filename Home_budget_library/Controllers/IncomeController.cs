using Home_budget_library.Models;

namespace Home_budget_library.Controllers
{
    public class IncomeController
    {
        private readonly HomeBudgetDbContext _context;
        public readonly CategoryController _categoryRepo;
        public IncomeController()
        {
            _context = new HomeBudgetDbContext();
            _categoryRepo = new CategoryController(_context);
        }
        public void Add(int userID, string title, decimal value, DateOnly date, string description, List<string>? categories)
        {
            Income income = new Income()
            {
                Title = title,
                UserID = userID,
                Value = value,
                date = date,
                Description = description,
            };
            _context.Incomes.Add(income);
            Save();
            int id = income.Id;

            if (categories is not null)
            {
                foreach (string name in categories)
                {

                }
            }
        }
        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }
    }
}
