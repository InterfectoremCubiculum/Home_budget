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
        public void Add(int userID, string title, decimal value, DateOnly date, string description, Dictionary<int, string>? categories)
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
            if( categories is not null)
                ConnectCategoriesIncome(income.Id,categories);
        }
        public void ConnectCategoriesIncome(int incomeID, Dictionary<int, string> categories)
        {
            foreach (var category in categories) {
                var incomeCategory = new IncomeCategory
                {
                    IncomeId = incomeID,
                    CategoryId = category.Key
                };
                _context.IncomeCategories.Add(incomeCategory);
            }
            _context.SaveChanges();
        }
        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public List<Income> GetAll(int userID) 
        {
            return _context.Incomes.Where(x => x.UserID == userID).ToList();
        }
    }
}
