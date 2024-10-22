using Home_budget_library.Models;

namespace Home_budget_library.Controllers
{
    public class TransactionController
    {
        protected readonly HomeBudgetDbContext _context;
        public readonly CategoryController _categoryRepo;
        public TransactionController()
        {
            _context = new HomeBudgetDbContext();
            _categoryRepo = new CategoryController(_context);
        }
        public void Add(int userID, string title, decimal value, DateOnly date, string description, Dictionary<int, string>? categories)
        {
            Transaction transaction = new Transaction()
            {
                Title = title,
                UserID = userID,
                Value = value,
                date = date,
                Description = description,
            };
            _context.Transactions.Add(transaction);
            Save();
            if (categories is not null)
                ConnectCategoriesTransaction(transaction.Id, categories);
        }
        public void ConnectCategoriesTransaction(int transactionID, Dictionary<int, string> categories)
        {
            foreach (var category in categories)
            {
                var transactionCategory = new TransactionCategory
                {
                    TransactionId = transactionID,
                    CategoryId = category.Key
                };
                _context.TransactionCategories.Add(transactionCategory);
            }
            _context.SaveChanges();
        }
        public void DeleteMany(List<int> indexes, int userId, bool isThisTransactionId = false)
        {
            if (isThisTransactionId)
            {
                var transactionsToRemove = _context.Transactions.Where(t => indexes.Contains(t.Id)).ToList();
                if (transactionsToRemove.Any())
                {
                    _context.Transactions.RemoveRange(transactionsToRemove);
                    _context.SaveChanges();
                }
            }
            else
            {
                var dicTransactions = GetAll(userId);
                var transactionList = dicTransactions.Keys.ToList();

                var transactionsToRemove = indexes
                    .Where(i => i > 0 && i <= transactionList.Count)
                    .Select(i => dicTransactions[i]).ToList();
                _context.Transactions.RemoveRange(transactionsToRemove);
                _context.SaveChanges();
            }
        }

        public void Edit(Transaction transaction, string newTitle, decimal newValue, DateOnly newDate, string newDescription, Dictionary<int, string>? categories)
        {
            transaction.Title = newTitle;
            transaction.Description = newDescription;
            transaction.date = newDate;
            transaction.Value = newValue;
            if (categories is not null)
            {
                _categoryRepo.DeleteByTransactionID(transaction.Id);
                ConnectCategoriesTransaction(transaction.Id, categories);
            }
            Save();
        }
        public bool Save()
        {
            return _context.SaveChanges() > 0 ? true : false;
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
                .Where(kvp => kvp.Value.Title.ToLower().Contains(title.ToLower()))
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        }


        public Transaction Copy(int userID, int transactionId)
        {
            var transaction = Get(userID, transactionId);
            var newtransaction = new Transaction()
            {
                Title = transaction.Title,
                UserID = userID,
                Value = transaction.Value,
                date = transaction.date,
                Description = transaction.Description,
            };
            _context.Transactions.Add(newtransaction);
            Save();

            return newtransaction;
        }
    }
}
