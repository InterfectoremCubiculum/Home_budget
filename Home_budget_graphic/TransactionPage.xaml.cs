using Home_budget_graphic.Domain;
using Home_budget_library.Controllers;
using Home_budget_library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Home_budget_graphic
{
    /// <summary>
    /// Logika interakcji dla klasy TransactionPage.xaml
    /// </summary>
    public partial class TransactionPage : Page
    {
        public List<TransactionItem> TransactionItemList { get; set; }
        public List<string> CategoryItemList { get; set; }
        public static TransactionController _controller = new TransactionController();
        public ICommand DeleteCommand { get; }
        public ICommand CopyCommand { get; }
        public TransactionPage()
        {
            InitializeComponent();
            TransactionItemList = new List<TransactionItem>();
            foreach (var tran in _controller.GetAll(MainWindow.LoggedInUser))
            {
                TransactionItemList.Add(new TransactionItem
                {
                    Id = tran.Key,
                    Title = tran.Value.Title,
                    Value = tran.Value.Value,
                    Description = tran.Value.Description,
                    Date = tran.Value.date.ToDateTime(TimeOnly.MinValue),
                    Category = _controller.GetCategoryName(tran.Value.CategoryID)
                });
            }
            CategoryItemList = _controller.GetAllCategories().Select(c => c.Name).ToList();
            // Tworzenie komendy Delete
            DeleteCommand = new RelayCommand(DeleteSelectedItems);

            // Tworzenie komendy Copy
            CopyCommand = new RelayCommand(CopySelectedItems);
            DataContext = this;
        }
        private void Transaction_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            TransactionEdit.Transaction_RowEditEnding(sender, e, _controller);
        }

        private void DataGrid_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            MouseWheelHandler.HandlePreviewMouseWheel(sender, e); // Wywołaj metodę pomocniczą
        }
        private void DeleteSelectedItems()
        {
            List<int> indexes = new List<int>();
            var itemsToDelete = TransactionItemList.Where(item => item.IsSelected).ToList();
            foreach (var item in itemsToDelete)
            {
                indexes.Add(item.Id);
                TransactionItemList.Remove(item);
            }
            _controller.DeleteMany(indexes, MainWindow.LoggedInUser);
        }

        private void CopySelectedItems()
        {
            var itemsToCopy = TransactionItemList.Where(item => item.IsSelected).ToList();
        }
    }
}
