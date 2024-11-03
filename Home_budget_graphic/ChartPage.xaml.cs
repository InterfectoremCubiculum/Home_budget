using Home_budget_graphic.BarChart;
using Home_budget_library.Controllers;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Home_budget_graphic
{
    /// <summary>
    /// Logika interakcji dla klasy ChartPage.xaml
    /// </summary>
    public partial class ChartPage : Page, INotifyPropertyChanged
    {
        private enum Months
        {
            January = 1,
            February = 2,
            March = 3,
            April = 4,
            May = 5,
            June = 6,
            July = 7,
            August = 8,
            September = 9,
            October = 10,
            November = 11,
            December = 12
        }
        private static readonly TransactionController _controller = new();
        private readonly ChartControl chartControlIncomes;
        private readonly ChartControl chartControlExpenses;
        private Dictionary<Tuple<int, int>, List<decimal>> dicYearMonth_values;
        public event PropertyChangedEventHandler PropertyChanged;

        private int _year = DateTime.Now.Year;
        public int Year
        {
            get => _year;
            set
            {
                _year = value;
                OnPropertyChanged(nameof(Year));
            }
        }

        public ChartPage()
        {
            InitializeComponent();
            chartControlIncomes = chartIncomes;
            chartControlExpenses = chartExpenses;
            dicYearMonth_values = _controller.GetAll(MainWindow.LoggedInUser)
                .Select(pair => pair.Value)
                .GroupBy(transaction => new { transaction.date.Year, transaction.date.Month })
                .ToDictionary(
                    group => Tuple.Create(group.Key.Year, group.Key.Month), // Use Tuple as key
                    group => new List<decimal>
                    {
                        group.Where(transaction => transaction.Value > 0).Sum(transaction => transaction.Value),
                        group.Where(transaction => transaction.Value < 0).Sum(transaction => transaction.Value)
                    }
            );
            WriteToChartBar(chartControlIncomes, Year, true);
            WriteToChartBar(chartControlExpenses, Year);
            DataContext = this;
        }

        private void WriteToChartBar(ChartControl chartControl, int year, bool WriteIncomes = false)
        {
            chartControl.SetMaxValue(GetMaxValueForYear(year)).Clear();
            int x = WriteIncomes ? 0 : 1;
            SolidColorBrush brush = WriteIncomes ? Brushes.Green : Brushes.Red;
            foreach (var month in Enum.GetValues(typeof(Months)))
            {
                if (dicYearMonth_values.TryGetValue(Tuple.Create(year, (int)month), out List<decimal> values))
                    chartControl.AddBar(Convert.ToDouble(Math.Abs(values[x])), $"{month}", brush);
                else
                    chartControl.AddBar(0, $"{month}", brush);
            }
        }
        private void PreviousYear_Click(object sender, RoutedEventArgs e)
        {
            Year -= 1;
            WriteToChartBar(chartControlIncomes, Year, true);
            WriteToChartBar(chartControlExpenses, Year);
        }
        private void NextYear_Click(object sender, RoutedEventArgs e)
        {
            Year += 1;
            WriteToChartBar(chartControlIncomes, Year, true);
            WriteToChartBar(chartControlExpenses, Year);
        }
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        public double GetMaxValueForYear(int year)
        {
            decimal maxValue = 0;

            foreach (var key in dicYearMonth_values.Keys)
            {
                if (key.Item1 == year)
                {
                    List<decimal> values = dicYearMonth_values[key];

                    decimal positiveValue = values[0];
                    decimal negativeValue = values[1];

                    maxValue = Math.Max(maxValue, Math.Max(positiveValue, Math.Abs(negativeValue)));
                }
            }
            return (double)maxValue;
        }
    }
}
