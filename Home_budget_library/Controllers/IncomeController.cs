using Home_budget_library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Home_budget_library.Controllers
{
    public class IncomeController
    {
        private readonly HomeBudgetDbContext _context;
        public IncomeController() 
        {
            _context = new HomeBudgetDbContext();
        }
        public void AddIncome(int userID, decimal value, DateTime date, string description, List<string>? categories) {
            Income income = new Income() 
            {
                UserID = userID,
                Value = value,
                date = date,
                Description = description,
            };
            _context.Incomes.Add(income);
            int id = income.Id;
           /* bool Saved = Save();
            if (categories is not null) {
                foreach( string name in categories)
                {
                    
                }
            }*/
        }
        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }
    }
}
