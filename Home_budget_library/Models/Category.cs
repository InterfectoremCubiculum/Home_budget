﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Home_budget_library.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        public string name { get; set; }
    }
}
