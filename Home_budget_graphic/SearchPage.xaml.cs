using Home_budget_graphic.Domain;
using Home_budget_library.Controllers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static MaterialDesignThemes.Wpf.Theme;
using TextBox = System.Windows.Controls.TextBox;

namespace Home_budget_graphic
{
    /// <summary>
    /// Logika interakcji dla klasy SearchPage.xaml
    /// </summary>
    public partial class SearchPage : Page
    {
        public List<TransactionItem> TransactionItemList { get; set; }
        public List<string> CategoryItemList { get; set; }
        public static TransactionController _controller = new TransactionController();
        public SearchPage()
        {
            InitializeComponent();
            CategoryItemList = _controller.GetAllCategories().Select(c => c.Name).ToList();
            Categories.ItemsSource = CategoryItemList;

        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                var textBox = sender as TextBox;
                string searchText = textBox?.Text;

                if (!string.IsNullOrEmpty(searchText))
                {
                    HandleSearch(searchText);

                    textBox.Clear();
                }

                e.Handled = true;
            }
        }
        private void HandleSearch(string searchText)
        {
            TransactionItemList = new List<TransactionItem>();
            foreach (var tran in _controller.Search(searchText, MainWindow.LoggedInUser))
            {
                TransactionItemList.Add(new TransactionItem
                {
                    Id = tran.Key,
                    Title = tran.Value.Title,
                    Value = tran.Value.Value,
                    Description = tran.Value.Description,
                    Date = tran.Value.date.ToString(),
                    Category = _controller.GetCategoryName(tran.Value.CategoryID)
                });
            }
            TransationsDataGrid.ItemsSource = TransactionItemList;
        }
        private void DataGrid_PreviewMouseWheel(object sender, MouseWheelEventArgs e) 
        {
            MouseWheelHandler.HandlePreviewMouseWheel(sender, e);
        }
        private void Transaction_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            TransactionEdit.Transaction_RowEditEnding( sender,  e, _controller);
        }
    }
}
