using Home_budget_graphic.Domain;
using Home_budget_library.Controllers;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Input;
namespace Home_budget_graphic
{
    /// <summary>
    /// Logika interakcji dla klasy TransactionPage.xaml
    /// </summary>
    public partial class TransactionPage : Page
    {
        private readonly static TransactionController _controller = new();
        private ObservableCollection<TransactionItem> transactionItemList = new ObservableCollection<TransactionItem>();
        public ICommand DeleteCommand { get; private set; }
        public ICommand CopyCommand { get; private set; }
        public TransactionPage()
        {
            InitializeComponent();
            DeleteCommand = new RelayCommand(DeleteSelectedItems);
            CopyCommand = new RelayCommand(CopySelectedItems);

            DataContext = this;
            GetTransactionList();
            TransationsDataGrid.ItemsSource = transactionItemList;
            Categories.ItemsSource = _controller.GetAllCategories().Select(c => c.Name).ToList();
        }

        private void DeleteSelectedItems()
        {
            var indexes = transactionItemList
                .Where(item => item.IsSelected)
                .Select(item => item.Id);

            if (!indexes.Any()) return;

            _controller.DeleteMany(indexes, MainWindow.LoggedInUser);
            RefreshTransactionList();
        }

        private void CopySelectedItems()
        {
            if (transactionItemList?.Any(item => item.IsSelected) != true)
                return;
            transactionItemList
                .Where(item => item.IsSelected)
                .ToList()
                .ForEach(item => _controller.Copy(MainWindow.LoggedInUser, item.Id));
            RefreshTransactionList();
        }

        private void GetTransactionList()
        {
            transactionItemList.Clear();
            foreach (var tran in _controller.GetAll(MainWindow.LoggedInUser))
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
        }
        private void RefreshTransactionList()
        {
            GetTransactionList();

            TransationsDataGrid.ItemsSource = null;
            TransationsDataGrid.ItemsSource = transactionItemList;
        }
        private void Transaction_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            TransactionEdit.Transaction_RowEditEnding(sender, e, _controller);
        }
        private void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            TransactionEdit.DatePicker_SelectedDateChanged(sender, e, _controller);
        }
        private void DataGrid_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            MouseWheelHandler.HandlePreviewMouseWheel(sender, e); // Wywołaj metodę pomocniczą
        }
    }
}
