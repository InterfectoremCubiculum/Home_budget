﻿using Home_budget_library.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Home_budget.Views
{
    public class IncomeView
    {
        private readonly IncomeController _controller;
        public IncomeView(IncomeController controller) {
            _controller = controller;
        }
        public void OnStart()
        {
            _controller.AddIncome(1, 21, new DateTime(), "dsdsadadasdasda sdsadsada sadsada", new List<string>() { "sadsa", "dsadasdsa" });
        }
    }
}
