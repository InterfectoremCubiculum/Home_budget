using Home_budget_graphic.Domain;
using Home_budget_library.Controllers;
using MaterialDesignThemes.Wpf;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Navigation;


namespace Home_budget_graphic
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public List<NavItem> NavList { get; set; }
        public static int LoggedInUser { get; private set; }
        private static UserController _userController;
        private static PaletteHelper _paletteHelper;

        public MainWindow()
        {

            InitializeComponent();
            _userController = new UserController();
            LoggedInUser = -1;
            UpdateNavigationButtons();
            MainFrame.Navigated += MainFrame_Navigated;
            SnackbarActive.SetSnackBar(Snackbar);
            _paletteHelper = new PaletteHelper();
            var theme = _paletteHelper.GetTheme();

            DarkModeToggleButton.IsChecked = theme.GetBaseTheme() == BaseTheme.Dark;

            DataContext = this;
            NavList =
            [
                new NavItem
                {
                    Title = "Home",
                    SelectedIcon = PackIconKind.Home,
                    UnselectedIcon = PackIconKind.HomeOutline,
                },
                new NavItem
                {
                    Title = "Transactions",
                    SelectedIcon = PackIconKind.AccountCash,
                    UnselectedIcon = PackIconKind.AccountCashOutline,
                },
                new NavItem
                {
                    Title = "Search",
                    SelectedIcon = PackIconKind.CardSearch,
                    UnselectedIcon = PackIconKind.CardSearchOutline,
                },
                new NavItem
                {
                    Title = "Calendar",
                    SelectedIcon = PackIconKind.CalendarMonth,
                    UnselectedIcon = PackIconKind.CalendarMonthOutline,
                },
                new NavItem
                {
                    Title = "Chart Bar",
                    SelectedIcon = PackIconKind.ChartBar,
                    UnselectedIcon = PackIconKind.ChartBar,
                },
            ];
        }
        private void NavList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ListBox listbox)
                switch (listbox.SelectedIndex)
                {
                    case 0:
                        MainFrame.Navigate(new HomePage());
                        break;
                    case 1:
                        MainFrame.Navigate(new TransactionPage());
                        break;
                    case 2:
                        MainFrame.Navigate(new SearchPage());
                        break;
                    case 3:
                        MainFrame.Navigate(new CalendarPage());
                        break;
                    case 4:
                        MainFrame.Navigate(new ChartPage());
                        break;
                }
        }




        private void MenuToggleButton_OnClick(object sender, RoutedEventArgs e) { }
        private void MenuDarkModeButton_Click(object sender, RoutedEventArgs e)
            => ModifyTheme(DarkModeToggleButton.IsChecked == true);
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (MainFrame.CanGoBack)
                MainFrame.GoBack();
        }

        private void ForwardButton_Click(object sender, RoutedEventArgs e)
        {
            if (MainFrame.CanGoForward)
                MainFrame.GoForward();
        }
        private void MainFrame_Navigated(object sender, NavigationEventArgs e)
        {
            UpdateNavigationButtons();
        }
        private void UpdateNavigationButtons()
        {
            BackButton.IsEnabled = MainFrame.CanGoBack;
            ForwardButton.IsEnabled = MainFrame.CanGoForward;
        }
        private void LogOut_Click(object sender, RoutedEventArgs e)
        {
            LoggedInUser = -1;
            LoginGrid.Visibility = Visibility.Visible;
            AppGrid.Visibility = Visibility.Hidden;

        }
        private void TerminalStart_Click(object sender, RoutedEventArgs e) 
        {
            try
            {
                var startInfo = new ProcessStartInfo
                {
                    FileName = "Home_budget.exe",
                    UseShellExecute = true
                };
                Process.Start(startInfo);

                Environment.Exit(0);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Console app error, not found in the same folder {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private async void Login_Click(object sender, RoutedEventArgs e)
        {
            string username = LoginUsernameTextBox.Text;
            string password = LoginPasswordBox.Password;
            if (_userController.ValidateLogin(username, password))
            {
                LoggedInUser = _userController.GetUserID(username);
                LoginGrid.Visibility = Visibility.Hidden;
                AppGrid.Visibility = Visibility.Visible;
                await ClearAllTextBoxes(LoginGrid);
                MainFrame.Navigate(new HomePage());
            }
            else
                SnackbarActive.WritToSnackbar("There is no such user or the password is incorrect");
        }
        private async void CreateAccount_Click(object sender, RoutedEventArgs e)
        {
            string username = CreateAccountUsernameTextBox.Text;
            string password = CreateAccountPasswordBox.Password;
            string repeatPassword = CreateAccountPasswordBox2.Password;
            _userController.Create_User_Check_UserName(username);
            switch (_userController.Create_User_Check_UserName(username))
            {
                case 0:
                    break;
                case 1:
                    SnackbarActive.WritToSnackbar("Too short username");
                    return;
                case 2:
                    SnackbarActive.WritToSnackbar("Username is not available");
                    return;
                case 3:
                    SnackbarActive.WritToSnackbar("Too short username and also not available");
                    return;
                default:
                    break;
            }
            if (!_userController.Create_User_Check_Password(password))
                SnackbarActive.WritToSnackbar("Too short password");
            else if (password != repeatPassword)
                SnackbarActive.WritToSnackbar("Passwords are diffrent");
            else
            {
                _userController.AddUser(username, password);
                LoggedInUser = _userController.GetUserID(username);
                LoginGrid.Visibility = Visibility.Hidden;
                AppGrid.Visibility = Visibility.Visible;
                await ClearAllTextBoxes(LoginGrid);
            }


        }
        public static void ModifyTheme(bool isDarkTheme)
        {
            var theme = _paletteHelper.GetTheme();

            theme.SetBaseTheme(isDarkTheme ? BaseTheme.Dark : BaseTheme.Light);
            _paletteHelper.SetTheme(theme);
        }
        private static async Task ClearAllTextBoxes(DependencyObject parent)
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                if (child is TextBox textBox)
                    textBox.Text = "";
                else if (child is PasswordBox passwordBox)
                    passwordBox.Password = "";
                else
                    await ClearAllTextBoxes(child);
            }
        }
    }
}