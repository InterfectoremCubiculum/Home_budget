using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Home_budget_graphic.Domain
{
    public static class SnackbarActive
    {
        private static Snackbar _snackbar;
        public static void SetSnackBar(Snackbar snackbar) 
        {
            _snackbar = snackbar;
        }
        public static void WritToSnackbar(string message, float snackbarDuration = 2)
        {
            _snackbar.MessageQueue?.Enqueue(
               $"{message}",
               null,
               null,
               null,
               false,
               true,
               TimeSpan.FromSeconds(snackbarDuration));
        }
    }
}
