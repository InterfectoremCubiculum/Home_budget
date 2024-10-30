using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using Home_budget_library.Controllers;

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
                        var transactionFromModel = controller.Get(MainWindow.loggedInUser, updatedTransaction.Id);
                        controller.Edit(
                            transactionFromModel,
                            updatedTransaction.Title,
                            updatedTransaction.Value,
                            DateOnly.Parse(updatedTransaction.Date),
                            updatedTransaction.Description,
                            controller.GetCategoryId(updatedTransaction.Category)
                        );
                    }
                }), System.Windows.Threading.DispatcherPriority.Background);
            }
        }
    }
}
