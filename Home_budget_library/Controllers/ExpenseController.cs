using Home_budget_library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Home_budget_library.Controllers
{
    public class ExpenseController
    {
        private readonly HomeBudgetDbContext _context;
        public ExpenseController()
        {
            _context = new HomeBudgetDbContext();
        }
    }
}
