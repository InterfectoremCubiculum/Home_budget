using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Home_budget_graphic.Domain
{
    public class TransactionItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public decimal Value { get; set; }
        public string Date { get; set; }
        public string Description { get; set; }

        public string Category { get; set; }

    }
}
