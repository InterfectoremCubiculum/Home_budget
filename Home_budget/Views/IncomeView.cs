using Home_budget_library.Controllers;
using Home_budget_library.Models;
using Spectre.Console;

namespace Home_budget.Views
{
    public class IncomeView : TransactionView
    {
        private static Panel navPanel;
        private readonly IncomeController _controller;
        public IncomeView(IncomeController controller)
        {
            _controller = controller;
            navPanel = new Panel(
                    Align.Center(
                        new Columns(
                            new Markup($"[{StyleClass.T_COL_STR}]Add->(1)[/]").Centered(),
                            new Markup($"[{StyleClass.T_COL_STR}]Delete->(2)[/]").Centered(),
                            new Markup($"[{StyleClass.T_COL_STR}]Copy and Edit->(3)[/]").Centered(),
                            new Markup($"[{StyleClass.T_COL_STR}]Edit->(4)[/]").Centered(),
                            new Markup($"[{StyleClass.T_COL_STR}]View All->(5)[/]").Centered(),
                            new Markup($"[{StyleClass.T_COL_STR}]Search->(6)[/]").Centered(),
                            new Markup($"[{StyleClass.T_COL_STR}]Main Menu->(7)[/]").Centered()
                            )
                        )
                    ).BorderColor(StyleClass.BORDER_COLOR)
                    .RoundedBorder();
        }
        public void OnStart()
        {
            Menu();
        }

        private void Menu()
        {

            BaseView();

            ConsoleKey key;
            while (true)
            {
                AnsiConsole.Cursor.Hide();
                key = Console.ReadKey(true).Key;
                switch (key)
                {
                    case ConsoleKey.D1: //Add
                        AnsiConsole.Cursor.Show();
                        Add();
                        Console.Clear();
                        BaseView();
                        break;
                    case ConsoleKey.D2: //Delete
                        AnsiConsole.Cursor.Show();
                        Delete();
                        Console.Clear();
                        BaseView();
                        break;
                    case ConsoleKey.D3: //Copy and Edit
                        ViewAll(25);
                        Copy();
                        Console.Clear();
                        BaseView();
                        break;
                    case ConsoleKey.D4: //Edit
                        ViewAll(25);
                        Edit();
                        break;
                    case ConsoleKey.D5: //View All
                        Console.Clear();
                        AnsiConsole.Write(navPanel);
                        ViewAll(null);
                        AnsiConsole.Write(navPanel);

                        break;
                    case ConsoleKey.D6: //Search
                        AnsiConsole.Cursor.Show();
                        Search();
                        break;
                    case ConsoleKey.D7: //Main Menu
                        AnsiConsole.Clear();
                        return;
                }
            }
        }

        private void Edit()
        {
            StyleClass.WriteDivider("Title");
        }           
        private void Search() 
        {
            StyleClass.WriteDivider("Search by title");
            var title = AnsiConsole.Ask<string>($"[{StyleClass.T_COL_STR}]Write [{StyleClass.T_HL}]Title[/][/]:");
            var result = _controller.Search(title, Program.loggedUserID);
            AnsiConsole.Clear();
            AnsiConsole.Write(navPanel);
            WriteToTable(result, int.MaxValue);
            AnsiConsole.Write(navPanel);
        }

        private void Copy()
        {
            throw new NotImplementedException();
        }

        protected void Add()

        {
            StyleClass.WriteDivider("Title");
            var title = AnsiConsole.Ask<string>($"[{StyleClass.T_COL_STR}]Write [{StyleClass.T_HL}]Title[/][/]:");

            StyleClass.WriteDivider("Value");
            var value = AnsiConsole.Ask<decimal>($"[{StyleClass.T_COL_STR}]Enter the [{StyleClass.T_HL}]Value[/][/]:");

            StyleClass.WriteDivider("Date");
            var date = AnsiConsole.Prompt(
                new TextPrompt<DateOnly>($"[{StyleClass.T_COL_STR}]Enter the [{StyleClass.T_HL}]Date[/] in default is today date [grey]yyyy.MM.dd[/][/]:")
                    .PromptStyle("green")
                    .ValidationErrorMessage($"[{StyleClass.T_HL_ERR_STR}]Please use this format yyyy.MM.dd[/]")
                    .DefaultValue(DateOnly.FromDateTime(DateTime.Today)));

            StyleClass.WriteDivider("string");
            var description = AnsiConsole.Ask<string>($"[{StyleClass.T_COL_STR}]Tell more [{StyleClass.T_HL}]Description[/][/]");

            _controller.Add(Program.loggedUserID, title, value, date, description, SelectedCategories());
        }
        private void Delete()
        {
            var idToDelete = AnsiConsole.Ask<string>($"[{StyleClass.T_COL_STR}]Write the ID of the items you want to remove. e.g. 1-5, 8, 11-13[/]:");
            var ids = ParseIdRanges(idToDelete);
            _controller.DeleteMany(ids, Program.loggedUserID);

        }
        private void ViewAll(int? maxLength)
        {
            var listOfIncomes = _controller.GetAll(Program.loggedUserID);
            if (maxLength is null)
            {
                maxLength = int.MaxValue;
            }

            WriteToTable(listOfIncomes, (int)maxLength);
        }

        private void WriteToTable(Dictionary<Income,int> listOfIncomes, int maxLength)
        {
            var table = new Table().Centered().BorderColor(StyleClass.BORDER_COLOR).RoundedBorder();
            AnsiConsole.Live(table)
                .AutoClear(false)
                .Start(context =>
                {
                    table.AddColumns(
                       $"[bold {Color.Purple}]Id[/]",
                       $"[bold {Color.Yellow}]Title[/]",
                       $"[bold {Color.Lime}]Value[/]",
                       $"[bold {Color.DarkMagenta}]Date[/]",
                       $"[bold {Color.Aqua}]Categories[/]",
                       $"[bold {Color.Silver}]Description[/]");
                    context.Refresh();


                    foreach (var item in listOfIncomes)
                    {
                        var categories = _controller._categoryRepo.GetByIncomeID(item.Key.Id);
                        var categoriesString = String.Join(", ", categories);
                        table.AddRow(
                            $"[{Color.Purple}]{item.Value}[/]",
                            $"[{Color.Yellow}]{SafeSubstring(item.Key.Title, 0, maxLength)}[/]",
                            $"[{Color.Lime}]{item.Key.Value}[/]",
                            $"[{Color.DarkMagenta}]{item.Key.date}[/]",
                            $"[{Color.Aqua}]{SafeSubstring(categoriesString, 0, maxLength)}[/]",
                            $"[{Color.Silver}]{SafeSubstring(item.Key.Description, 0, maxLength)}[/]");
                        context.Refresh();
                    }
                });
        }

        protected Dictionary<int, string> SelectedCategories()
        {
            return base.SelectedCategories(_controller._categoryRepo.GetAll());
        }
        public static List<int> ParseIdRanges(string input)
        {
            var ids = new List<int>();
            var parts = input.Split(',', StringSplitOptions.RemoveEmptyEntries);

            foreach (var part in parts)
            {
                if (part.Contains('-'))
                {
                    var rangeParts = part.Split('-');
                    if (rangeParts.Length == 2 && int.TryParse(rangeParts[0], out int start) && int.TryParse(rangeParts[1], out int end))
                    {
                        for (int i = start; i <= end; i++)
                        {
                            ids.Add(i);
                        }
                    }
                }
                else if (int.TryParse(part, out int singleId))
                {
                    ids.Add(singleId);
                }
            }
            return ids;
        }
        public static string SafeSubstring(string str, int startIndex, int length)
        {
            if (startIndex < 0 || startIndex >= str.Length)
                return string.Empty;

            if (startIndex + length > str.Length)
            {
                length = str.Length - startIndex;
                return str.Substring(startIndex, length);
            }
            else
                return str.Substring(startIndex, length) + "...";
        }

        private void BaseView() 
        {
            AnsiConsole.Write(navPanel);
            ViewAll(35);
            AnsiConsole.Write(navPanel);
        }
    }
}
