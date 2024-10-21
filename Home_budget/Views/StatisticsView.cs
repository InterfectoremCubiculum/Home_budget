using Home_budget_library.Controllers;
using Spectre.Console;

namespace Home_budget.Views
{
    class StatisticsView
    {
        private readonly StatisticsController _controller;
        private readonly Panel navPanel;

        public StatisticsView(StatisticsController controller)
        {
            _controller = controller;
            navPanel = CreateNavPanel();
        }

        public void OnStart()
        {
            Menu();
        }
        private void Menu()
        {
            StyleClass.ClearWrite([navPanel]);
            ConsoleKey key;
            while (true)
            {
                AnsiConsole.Cursor.Hide();
                key = Console.ReadKey(true).Key;
                switch (key)
                {
                    case ConsoleKey.D1: //Calendar
                        StyleClass.ClearWrite([navPanel]);
                        ShowCalendar();
                        break;
                    case ConsoleKey.D2: //Barchart
                        StyleClass.ClearWrite([navPanel]);
                        YearBarchartTransactions();
                        break;
                    case ConsoleKey.D3: //Main Menu
                        return;
                }
            }
        }
        private void ShowCalendar()
        {
            // Ask for month
            StyleClass.WriteDivider("Month");
            var month = AnsiConsole.Prompt(
                new TextPrompt<int>($"[{StyleClass.T_COL_STR}]Enter the [{StyleClass.T_HL_STR}]Month[/] in default is today month [grey](1-12)[/][/]:")
                    .ValidationErrorMessage($"[{StyleClass.T_HL_ERR_STR}]Please enther the month between (1-12)[/]")
                    .DefaultValue(DateTime.Today.Month)
                    .Validate(m => m >= 1 && m <= 12));

            StyleClass.WriteDivider("Year");
            var year = AnsiConsole.Prompt(
                new TextPrompt<int>($"[{StyleClass.T_COL_STR}]Enter the [{StyleClass.T_HL_STR}]Year[/] in default is today year [grey](yyyy)[/][/]:")
                    .ValidationErrorMessage($"[{StyleClass.T_HL_ERR_STR}]Please use this format (yyyy)[/]")
                    .DefaultValue(DateTime.Today.Year)
                    .Validate(y => y.ToString().Length == 4));



            var dicTransactions = _controller.GetTransactionValueEachDay(Program.loggedUserID, month, year);
            var calendarIncome = CreateCalendar(month, year, dicTransactions.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.positiveSum), Color.Green);
            var calendarExspense = CreateCalendar(month, year, dicTransactions.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.negativeSum), Color.Red);

            Panel calendarPanel = new Panel(
                Align.Center(
                new Rows(
                    StyleClass.Divider("Days with incomes"),
                    calendarIncome.NoBorder(),
                    StyleClass.Divider("Days with expenses"),
                    calendarExspense.NoBorder()))).BorderColor(StyleClass.BORDER_COLOR).RoundedBorder();

            StyleClass.ClearWrite([navPanel, calendarPanel]);
        }

        private static Calendar CreateCalendar(int month, int year, Dictionary<int, decimal> dicTransactions, Style color)
        {
            var calendar = new Calendar(year, month)
                .BorderColor(StyleClass.BORDER_COLOR)
                .RoundedBorder();
            calendar.HighlightStyle(color);
            foreach (var item in dicTransactions)
            {
                if (item.Value != 0)
                    calendar.AddCalendarEvent(year, month, item.Key);
            }
            return calendar;
        }

        private Panel CreateNavPanel()
        {
            return new Panel(
                Align.Center(
                    new Columns(
                        new Markup($"[{StyleClass.T_COL_STR}]View transactions by month ->(1)[/]").Centered(),
                        new Markup($"[{StyleClass.T_COL_STR}]Yearly bar chart->(2)[/]").Centered(),
                        new Markup($"[{StyleClass.T_COL_STR}]Main Menu->(3)[/]").Centered()
                    )
                )
            ).BorderColor(StyleClass.BORDER_COLOR).RoundedBorder();
        }
        private void YearBarchartTransactions()
        {
            StyleClass.WriteDivider("Year");
            var year = AnsiConsole.Prompt(
                new TextPrompt<int>($"[{StyleClass.T_COL_STR}]Enter the [{StyleClass.T_HL_STR}]Year[/] in default is today year [grey](yyyy)[/][/]:")
                    .ValidationErrorMessage($"[{StyleClass.T_HL_ERR_STR}]Please use this format (yyyy)[/]")
                    .DefaultValue(DateTime.Today.Year)
                    .Validate(y => y.ToString().Length == 4));

            var transactionEachMonth = _controller.GetTransactionValueEachMonth(Program.loggedUserID, year);

            StyleClass.ClearWrite([navPanel]);
            Panel barPanel = new Panel(
                Align.Center(
                new Rows(
                    StyleClass.Divider("Incomes in each month"),
                    new BarChart()
                        .CenterLabel()
                        .AddItems(transactionEachMonth, (item) => new BarChartItem(
                           IntToMonth(item.Key), (double)item.Value.positiveSum, StyleClass.Colors[item.Key - 1])),
                    new Text("\n"),
                    StyleClass.Divider("Expenses in each month"),
                    new BarChart()
                        .CenterLabel()
                        .AddItems(transactionEachMonth, (item) => new BarChartItem(
                            IntToMonth(item.Key), -1 * (double)item.Value.negativeSum, StyleClass.Colors[item.Key - 1]))

                ))).RoundedBorder().BorderColor(StyleClass.BORDER_COLOR).Padding(1, 1, 1, 1).Header($"Transactions of year: {year}", Justify.Center);
            AnsiConsole.Write(barPanel);
        }
        public string IntToMonth(int month)
        {
            return new DateTime(1, month, 1).ToString("MMMM");
        }

    }

}
