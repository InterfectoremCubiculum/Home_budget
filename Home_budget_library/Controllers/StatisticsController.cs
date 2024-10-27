using Home_budget_library.Models;

namespace Home_budget_library.Controllers
{
    public class StatisticsController
    {
        protected readonly HomeBudgetDbContext _context;
        public StatisticsController()
        {
            _context = new HomeBudgetDbContext();
        }
        public Dictionary<int, (decimal positiveSum, decimal negativeSum)> GetTransactionValueEachDay(int userID, int month, int year)
        {
            var transactions = _context.Transactions
                .Where(t => t.UserID == userID && t.date.Month == month && t.date.Year == year)
                .ToList();

            var transactionValues = new Dictionary<int, (decimal positiveSum, decimal negativeSum)>();

            foreach (var transaction in transactions)
            {
                int day = transaction.date.Day;
                if (!transactionValues.ContainsKey(day))
                    transactionValues[day] = (0, 0);
                if (transaction.Value > 0)
                    transactionValues[day] = (transactionValues[day].positiveSum + transaction.Value, transactionValues[day].negativeSum);
                else
                    transactionValues[day] = (transactionValues[day].positiveSum, transactionValues[day].negativeSum + transaction.Value);
            }
            return transactionValues;
        }
        public Dictionary<int, (decimal positiveSum, decimal negativeSum)>? GetTransactionValueEachMonth(int userID, int year)
        {
            var transactions = _context.Transactions
                .Where(t => t.UserID == userID && t.date.Year == year).OrderBy(t => t.date.Month)
                .ToList();

            var transactionValues = new Dictionary<int, (decimal positiveSum, decimal negativeSum)>();

            foreach (var transaction in transactions)
            {
                int month = transaction.date.Month;
                if (!transactionValues.ContainsKey(month))
                    transactionValues[month] = (0, 0);
                if (transaction.Value > 0)
                    transactionValues[month] = (transactionValues[month].positiveSum + transaction.Value, transactionValues[month].negativeSum);
                else
                    transactionValues[month] = (transactionValues[month].positiveSum, transactionValues[month].negativeSum + transaction.Value);
            }
            return transactionValues.Count == 0 ? null : transactionValues;
        }
    }
}
