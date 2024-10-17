using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Home_budget_library.Models
{
    public class Income
    {
        [Key]
        public int Id { get; set; }
        public int UserID {get; set; }
        public string Title { get; set; }

        public decimal Value { get; set; }

        public DateOnly date { get; set; }

        public string Description { get; set; }
    }
}
