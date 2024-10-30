using Home_budget_graphic.Domain;
using Home_budget_library;
using MaterialDesignThemes.Wpf;
using System.Windows;
using System.Configuration;
using System.Diagnostics;
using System.Threading;
using System.Windows.Media;
using MaterialDesignThemes.Wpf;
using Home_budget_library.Controllers;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using System.Windows.Controls;
using System.Windows.Navigation;
using ControlzEx.Theming;


namespace Home_budget_graphic
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public List<NavItem> NavList { get; set; }
        public static int loggedInUser { get; private set; }
        private static UserController _userController;

        public MainWindow()
        {

            InitializeComponent();
            _userController = new UserController();
            loggedInUser = -1;
            UpdateNavigationButtons();
            MainFrame.Navigated += MainFrame_Navigated;

            var paletteHelper = new PaletteHelper();
            var theme = paletteHelper.GetTheme();
            App app = (App)Application.Current;

            switch (app.InitialTheme)
            {
                case BaseTheme.Dark:
                    ModifyTheme(true);
                    break;
                case BaseTheme.Light:
                    ModifyTheme(false);
                    break;
            }

            DarkModeToggleButton.IsChecked = theme.GetBaseTheme() == BaseTheme.Dark;

            if (paletteHelper.GetThemeManager() is { } themeManager)
            {
                themeManager.ThemeChanged += (_, e)
                    => DarkModeToggleButton.IsChecked = e.NewTheme?.GetBaseTheme() == BaseTheme.Dark;
            }
            DataContext = this;
            NavList = new()
            {
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
            };
        }
        private void NavList_SelectionChanged(object sender, SelectionChangedEventArgs e) 
        {
            if (sender is ListBox listbox)
                switch (listbox.SelectedIndex ) 
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




        private async void MenuToggleButton_OnClick(object sender, RoutedEventArgs e) { }
        private async void MenuDarkModeButton_Click(object sender, RoutedEventArgs e)
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
            loggedInUser = -1;
            LoginGrid.Visibility = Visibility.Visible;
            AppGrid.Visibility = Visibility.Hidden;

        }
        private async void Login_Click(object sender, RoutedEventArgs e) 
        {
            string username = LoginUsernameTextBox.Text;
            string password = LoginPasswordBox.Password;
            if (_userController.ValidateLogin(username, password)) 
            {
                loggedInUser =  _userController.GetUserID(username);
                LoginGrid.Visibility = Visibility.Hidden;
                AppGrid.Visibility = Visibility.Visible;
                await ClearAllTextBoxes(LoginGrid);
                MainFrame.Navigate(new HomePage());
            }
            else
                await SnackbarAtive("There is no such user or the password is incorrect");
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
                    await SnackbarAtive("Too short username");
                    return;
                case 2:
                    await SnackbarAtive("Username is not available");
                    return;
                case 3:
                    await SnackbarAtive("Too short username and also not available");
                    return;
                default:
                    break;
            }
            if (!_userController.Create_User_Check_Password(password))
                await SnackbarAtive("Too short password");
            else if (password != repeatPassword)
                await SnackbarAtive("Passwords are diffrent");
            else 
            {
                _userController.AddUser(username, password);
                loggedInUser = _userController.GetUserID(username);
                LoginGrid.Visibility = Visibility.Hidden;
                AppGrid.Visibility = Visibility.Visible;
                await ClearAllTextBoxes(LoginGrid);
            }


        }
        private async Task SnackbarAtive(string message, float snackbarDuration=3) 
        {
            Snackbar.MessageQueue?.Enqueue(
               $"{message}",
               null,
               null,
               null,
               false,
               true,
               TimeSpan.FromSeconds(snackbarDuration));
        }
        private static void ModifyTheme(bool isDarkTheme)
        {
            var paletteHelper = new PaletteHelper();
            var theme = paletteHelper.GetTheme();

            theme.SetBaseTheme(isDarkTheme ? BaseTheme.Dark : BaseTheme.Light);
            paletteHelper.SetTheme(theme);
        }
        private async Task ClearAllTextBoxes(DependencyObject parent)
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