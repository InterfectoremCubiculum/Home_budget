using Home_budget_library.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Home_budget.Views
{
    public class ExpenseView
    {
        private readonly IncomeController _controller;
        public ExpenseView(IncomeController controller)
        {
            _controller = controller;
        }
        public void OnStart()
        {

        }
    }
}
