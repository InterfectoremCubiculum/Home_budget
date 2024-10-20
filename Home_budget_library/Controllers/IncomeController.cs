using Home_budget_library.Models;

namespace Home_budget_library.Controllers
{
    public class IncomeController
    {
        private readonly HomeBudgetDbContext _context;
        public readonly CategoryController _categoryRepo;

        public List<int> incomesList { get; private set; }
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
            if (categories is not null)
                ConnectCategoriesIncome(income.Id, categories);
        }

        public void ConnectCategoriesIncome(int incomeID, Dictionary<int, string> categories)
        {
            foreach (var category in categories)
            {
                var incomeCategory = new IncomeCategory
                {
                    IncomeId = incomeID,
                    CategoryId = category.Key
                };
                _context.IncomeCategories.Add(incomeCategory);
            }
            _context.SaveChanges();
        }
        public void DeleteMany(List<int> indexes, int userId, bool isThisIncomesId = false)
        {
            if (isThisIncomesId)
            {
                var incomesToRemove = _context.Incomes.Where(i => indexes.Contains(i.Id));
                if (incomesToRemove.Any())
                {
                    _context.Incomes.RemoveRange(incomesToRemove);
                    _context.SaveChanges();
                }
            }
            else
            {
                var incomes = GetAll(userId);
                var incomesList = incomes.Keys.ToList();

                var incomesToRemove = indexes
                    .Where(i => i > 0 && i <= incomesList.Count)
                    .Select(i => incomesList[i - 1]);

                _context.Incomes.RemoveRange(incomesToRemove);
                _context.SaveChanges();

            }
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public Dictionary<Income, int> GetAll(int userID)
        {
            var allIncomes = _context.Incomes
                 .Where(x => x.UserID == userID)
                 .ToList();

            return allIncomes.Select((income, index) => new { income, relativeValue = index + 1 })
                             .ToDictionary(x => x.income, x => x.relativeValue);
        }

        public Dictionary<Income, int> Search(string title, int userID)
        {
            var allIncomes = GetAll(userID);
            return allIncomes.Where(kvp => kvp.Key.Title.Contains(title))
                             .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        }
        public Income Get(int Id)
        {
            return _context.Incomes.First(x => x.Id == Id);
        }
    }
}
