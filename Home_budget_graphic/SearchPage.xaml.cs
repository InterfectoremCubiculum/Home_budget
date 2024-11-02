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
        private List<TransactionItem> transactionItemList { get; set; }
        private static TransactionController _controller = new();
        public ICommand DeleteCommand { get; }
        public ICommand CopyCommand { get; }
        private string searchText;
        public SearchPage()
        {
            InitializeComponent();
            DeleteCommand = new RelayCommand(DeleteSelectedItems);
            CopyCommand = new RelayCommand(CopySelectedItems);

            Categories.ItemsSource = _controller.GetAllCategories().Select(c => c.Name).ToList();
            DataContext = this;
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
                    this.searchText = searchText;
                    textBox.Clear();
                }
                e.Handled = true;
            }
        }
        private void HandleSearch(string searchText="")
        {
            transactionItemList = new();
            foreach (var tran in _controller.Search(searchText, MainWindow.LoggedInUser))
            {
                transactionItemList.Add(new TransactionItem
                {
                    Id = tran.Key,
                    Title = tran.Value.Title,
                    Value = tran.Value.Value,
                    Description = tran.Value.Description,
                    Date = tran.Value.date.ToDateTime(TimeOnly.MinValue),
                    Category = _controller.GetCategoryName(tran.Value.CategoryID)
                });
            }
            TransationsDataGrid.ItemsSource = null;
            TransationsDataGrid.ItemsSource = transactionItemList;
        }
        private void DeleteSelectedItems()
        {
            if (transactionItemList?.Any(item => item.IsSelected) != true)
                return;

            var indexes = transactionItemList
                .Where(item => item.IsSelected)
                .Select(item => item.Id)
                .ToList();

            _controller.DeleteMany(indexes, MainWindow.LoggedInUser);
            HandleSearch(searchText);
        }
        private void CopySelectedItems()
        {
            if (transactionItemList?.Any(item => item.IsSelected) != true)
                return;

            transactionItemList
                .Where(item => item.IsSelected)
                .ToList()
                .ForEach(item => _controller.Copy(MainWindow.LoggedInUser, item.Id));

            HandleSearch(searchText);
        }
        private void DataGrid_PreviewMouseWheel(object sender, MouseWheelEventArgs e) 
        {
            MouseWheelHandler.HandlePreviewMouseWheel(sender, e);
        }
        private void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            TransactionEdit.DatePicker_SelectedDateChanged(sender, e, _controller);
        }
        private void Transaction_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            TransactionEdit.Transaction_RowEditEnding( sender,  e, _controller);
        }
    }
}
