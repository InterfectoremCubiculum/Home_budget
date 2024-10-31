using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using Home_budget_library.Controllers;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Home_budget_graphic.Domain
{
    public static class TransactionEdit
    {
        public static void Transaction_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e, TransactionController controller)
        {
            if (e.EditAction == DataGridEditAction.Commit)
            {
                Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    var updatedTransaction = e.Row.Item as TransactionItem;
                    if (updatedTransaction != null)
                    {
                        var transactionFromModel = controller.Get(MainWindow.LoggedInUser, updatedTransaction.Id);
                        controller.Edit(
                            transactionFromModel,
                            updatedTransaction.Title,
                            updatedTransaction.Value,
                            DateOnly.FromDateTime(updatedTransaction.Date),
                            updatedTransaction.Description,
                            controller.GetCategoryId(updatedTransaction.Category)
                        );
                    }
                }), System.Windows.Threading.DispatcherPriority.Background);
            }
        }
    }
}
