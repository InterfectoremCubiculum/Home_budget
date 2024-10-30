using Home_budget_library.Models;
using System.Net.Http.Headers;

namespace Home_budget_library.Controllers
{
    public class TransactionController
    {
        protected readonly HomeBudgetDbContext _context;
        public TransactionController()
        {
            _context = new HomeBudgetDbContext();
        }
        public void Add(int userID, string title, decimal value, DateOnly date, string description, int categoryID)
        {
            Transaction transaction = new Transaction()
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

        public void Edit(Transaction transaction, string newTitle, decimal newValue, DateOnly newDate, string newDescription, int categoryID)
        {
            transaction.Title = newTitle;
            transaction.Description = newDescription;
            transaction.date = newDate;
            transaction.Value = newValue;
            transaction.CategoryID = categoryID;
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
        public bool ValidateValue(decimal value) 
        {

            return value == Math.Round(value, 2);
        }
    }
}