using ControlzEx.Standard;
using Home_budget_graphic.Domain;
using Home_budget_library.Controllers;
using System.Windows;
using System.Windows.Controls;

namespace Home_budget_graphic
{
    /// <summary>
    /// Logika interakcji dla klasy HomePage.xaml
    /// </summary>
    public partial class HomePage : Page
    {
        private static readonly TransactionController _controller = new();
        public HomePage()
        {
            InitializeComponent();
            Category.ItemsSource = _controller.GetAllCategories().Select(c => c.Name).ToList();
            Date.SelectedDate = DateTime.Now;
        }

        private void OnClick_AddNewTransaction(object sender, RoutedEventArgs e)
        {
            string messageToSnackBar = "";
            if (string.IsNullOrWhiteSpace(Title.Text))
                messageToSnackBar += ("Title is required.\n");

            if (!decimal.TryParse(Value.Text, out decimal transactionValue) || !(transactionValue == Math.Round(transactionValue, 2)))
                messageToSnackBar += ("Enter a valid decimal number for Value. Maximum of 2 decimal places\n");

            if (!Date.SelectedDate.HasValue)
                messageToSnackBar += ("Date is required.\n");

            if (Category.SelectedItem == null)
                messageToSnackBar += ("Category is required.\n");

            if (messageToSnackBar.Length <= 0)
            {
                _controller.Add(
                    MainWindow.LoggedInUser,
                    Title.Text,
                    transactionValue,
                    DateOnly.FromDateTime(Date.SelectedDate.Value),
                    Description.Text,
                    _controller.GetCategoryId(Category.SelectedItem as string)
                );
                messageToSnackBar = "Transaction added successfully.";
            }
            SnackbarActive.WritToSnackbar(messageToSnackBar, 4);
        }
        private void OnClick_ClearForms(object sender, RoutedEventArgs e)
        {
            Title.Clear();
            Value.Clear();
            Date.SelectedDate= DateTime.Now;
            Category.SelectedIndex = -1;
            Description.Clear();
        }
    }
}
