using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Home_budget_library.Models
{
    public class Expense
    {
        [Key]
        public int Id { get; set; }
        public int UserID { get; set; }
        public decimal Value { get; set; }

        public DateOnly date { get; set; }

        public string Description { get; set;}
    }
}
