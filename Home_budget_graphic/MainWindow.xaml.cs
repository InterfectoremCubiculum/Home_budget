using Home_budget_graphic.Domain;
using MaterialDesignThemes.Wpf;
using System.Windows;
using System.Configuration;
using System.Diagnostics;
using System.Threading;
using System.Windows.Media;
using MaterialDesignThemes.Wpf;


namespace Home_budget_graphic
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public List<NavItem> NavList { get; set; }
        public MainWindow()
        {

            InitializeComponent();
            MainFrame.Navigate(new HomePage());

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


        private async void MenuPopupButton_OnClick(object sender, RoutedEventArgs e) 
        {
        }
        private async void MenuToggleButton_OnClick(object sender, RoutedEventArgs e) { }
        private async void Button_Click(object sender, RoutedEventArgs e) { }
        private async void MenuDarkModeButton_Click(object sender, RoutedEventArgs e)
            => ModifyTheme(DarkModeToggleButton.IsChecked == true);
        private async void FlowDirectionButton_Click(object sender, RoutedEventArgs e) { }
        private async void LogOut_Click(object sender, RoutedEventArgs e) 
        {
            //new LoginWindow().Show();
            this.Close();
        }

        private static void ModifyTheme(bool isDarkTheme)
        {
            var paletteHelper = new PaletteHelper();
            var theme = paletteHelper.GetTheme();

            theme.SetBaseTheme(isDarkTheme ? BaseTheme.Dark : BaseTheme.Light);
            paletteHelper.SetTheme(theme);
        }

    }
}