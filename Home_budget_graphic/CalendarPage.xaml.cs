
using Home_budget_library.Controllers;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Calendar = System.Windows.Controls.Calendar;

namespace Home_budget_graphic
{
    /// <summary>
    /// Logika interakcji dla klasy CalendarPage.xaml
    /// </summary>
    public partial class CalendarPage : Page
    {
        private readonly Dictionary<DateOnly, List<Decimal>> keyDayValueTrans;
        private readonly static TransactionController _controller = new();
        private static bool displayPositiveValue = true;
        public CalendarPage()
        {
            InitializeComponent();
            keyDayValueTrans = _controller.ValuesByDays(MainWindow.LoggedInUser);
            CalendarSelectDays(DisplayCalendar, DateTime.Now, displayPositiveValue);
            DisplayCalendar.DataContext = this;

        }
        private void Calendar_DisplayDateChanged(object sender, CalendarDateChangedEventArgs e)
        {
            Calendar calObj = sender as Calendar;
            if (calObj.DisplayMode == CalendarMode.Year)
                CalendarSelectDays(calObj, e.AddedDate.GetValueOrDefault(), displayPositiveValue);
        }
        private void CalendarSelectDays(Calendar calendar, DateTime dateTime, bool displayPositiveValue)
        {
            calendar.SelectedDates.Clear();

            var daysToSelect = keyDayValueTrans
               .Where(pair => pair.Key.Year == dateTime.Year)
               .Where(pair => pair.Value.Any(value => displayPositiveValue ? value > 0 : value < 0))
               .Select(pair => pair.Key);

            foreach (var day in daysToSelect)
                calendar.SelectedDates.Add(day.ToDateTime(new TimeOnly(0, 0)));
        }

        private void CalendarModeToggleButton_Click(object sender, RoutedEventArgs e) 
        {
            displayPositiveValue = !displayPositiveValue;
            CalendarSelectDays(DisplayCalendar, DisplayCalendar.DisplayDate.Date, displayPositiveValue);
        }
    }
}
