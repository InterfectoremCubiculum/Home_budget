using Home_budget_library.Models;

namespace Home_budget_library.Controllers
{
    public class TransactionController
    {
        protected readonly HomeBudgetDbContext _context;
        public TransactionController()
        {
            _context = new HomeBudgetDbContext();
            AddCategories();
        }
        public void Add(int userID, string title, decimal value, DateOnly date, string description, int categoryID)
        {
            Transaction transaction = new()
            {
                Title = title,
                UserID = userID,
                Value = value,
                date = date,
                Description = description,
                CategoryID = categoryID
            };
            _context.Transactions.Add(transaction);
            Save();
        }
        public int GetCategoryId(string categoryName)
        {
            return _context.Categories.First(cat => cat.Name == categoryName).Id;
        }
        public string GetCategoryName(int categoryID)
        {
            return _context.Categories.First(cat => cat.Id == categoryID).Name;
        }

        public void DeleteMany(IEnumerable<int> indexes, int userId)
        {
            var dicTransactions = GetAll(userId);
            var transactionList = dicTransactions.Keys.ToList();

            var transactionsToRemove = indexes
                .Where(i => i > 0 && i <= transactionList.Count)
                .Select(i => dicTransactions[i]).ToList();
            _context.Transactions.RemoveRange(transactionsToRemove);
            _context.SaveChanges();
        }
        public Dictionary<DateOnly, List<Decimal>> ValuesByDays(int userID)
        {
            return _context.Transactions
                .Where(transaction => transaction.UserID == userID)
                .Select(transaction => new
                {
                    _Date = transaction.date,
                    _Value = transaction.Value
                })
                .GroupBy(atransaction => atransaction._Date)
                .ToDictionary(
                    group => group.Key,
                    group => group.Select(aTransaction => aTransaction._Value).ToList()
                );
        }
        public void Edit(Transaction transaction, string newTitle, decimal newValue, DateOnly newDate, string newDescription, int categoryID)
        {
            transaction.Title = newTitle;
            transaction.Description = newDescription;
            transaction.date = newDate;
            transaction.Value = newValue;
            transaction.CategoryID = categoryID;
            _context.Update(transaction);
            Save();
        }
        public bool Save()
        {
            return _context.SaveChanges() > 0;
        }
        public Transaction Get(int userID, int transactionID)
        {
            return GetAll(userID).GetValueOrDefault(transactionID);
        }
        public Dictionary<int, Transaction> GetAll(int userID)
        {
            var allTransactions = _context.Transactions
                .Where(x => x.UserID == userID)
                .ToList();

            var dicTransactions = allTransactions.Select((transaction, index) =>
                            new { transaction, relativeValue = index + 1 })
                            .ToDictionary(x => x.relativeValue, x => x.transaction);
            return dicTransactions;
        }
        public Dictionary<int, Transaction> Search(string title, int userID)
        {
            return GetAll(userID)
                .Where(kvp => kvp.Value.Title.Contains(title, StringComparison.CurrentCultureIgnoreCase))
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        }

        public Transaction? Copy(int userID, int transactionId)
        {
            var transaction = Get(userID, transactionId);
            if (transaction == null) return null;

            var newTransaction = new Transaction
            {
                Title = transaction.Title,
                UserID = userID,
                Value = transaction.Value,
                date = transaction.date,
                Description = transaction.Description,
                CategoryID = transaction.CategoryID,
            };

            _context.Transactions.Add(newTransaction);
            Save();

            return newTransaction;

        }
        public List<Category> GetAllCategories()
        {
            return _context.Categories.ToList();
        }




        public void AddCategories()
        {
            if (!_context.Categories.Any())
            {
                var categories = new List<Category>
                {
                    new Category { Name = "Groceries" },
                    new Category { Name = "Utilities" },
                    new Category { Name = "Entertainment" },
                    new Category { Name = "Transportation" },
                    new Category { Name = "Health" },
                    new Category { Name = "Rent" },
                    new Category { Name = "Food" },
                    new Category { Name = "Investments" },
                    new Category { Name = "Incomes" },
                    new Category { Name = "Gambling" }
                };
                _context.Categories.AddRange(categories);
                _context.SaveChanges();
            }
        }
    }
}