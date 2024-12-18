﻿using Home_budget_library.Controllers;
using System.Windows;
using System.Windows.Controls;

namespace Home_budget_graphic.Domain
{
    public static class TransactionEdit
    {
        public static void Transaction_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e, TransactionController controller)
        {
            if (sender is DataGrid dataGrid)
                if (dataGrid.CurrentColumn is DataGridCheckBoxColumn)
                    return;

            if (e.EditAction == DataGridEditAction.Commit)
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
        public static void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e, TransactionController controller)
        {
            if (sender is DatePicker datePicker)
            {
                var currentTransactionItem = datePicker.DataContext as TransactionItem;

                if (datePicker.SelectedDate.HasValue)
                {
                    var transactionFromModel = controller.Get(MainWindow.LoggedInUser, currentTransactionItem.Id);
                    controller.Edit(
                        transactionFromModel,
                        currentTransactionItem.Title,
                        currentTransactionItem.Value,
                        DateOnly.FromDateTime(datePicker.SelectedDate.Value),
                        currentTransactionItem.Description,
                        controller.GetCategoryId(currentTransactionItem.Category)
                    );
                }

            }
        }
    }
}
