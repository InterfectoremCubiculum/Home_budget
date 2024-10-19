using Home_budget_library.Controllers;
using Home_budget_library.Models;
using Spectre.Console;
using System.ComponentModel.DataAnnotations;

namespace Home_budget.Views
{
    public class IncomeView : TransactionView
    {
        private readonly IncomeController _controller;
        public IncomeView(IncomeController controller)
        {
            _controller = controller;
        }
        public void OnStart()
        {
            Menu();
        }

        private void Menu()
        {
            while (true)
            {
                var option = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Please select an option")
                    .PageSize(7)
                    .AddChoices(new[] { "Add", "Delete", "Copy and Edit", "Edit", "View All", "Search", "Main Menu" }));

                switch (option)
                {
                    case "Add":
                        Add();
                        break;
                    case "Delete":
                        ViewAll(25);
                        Delete();
                        break;
                    case "Copy":
                        ViewAll(25);
                        Copy();
                        break;
                    case "Edit":
                        ViewAll(25);
                        Edit();
                        break;
                    case "View All":
                        ViewAll(null);
                        break;
                    case "Search":
                        break;
                    case "Main Menu":
                        return;
                }
            }
        }

        private void Edit()
        {
            throw new NotImplementedException();
        }

        private void Copy()
        {
            throw new NotImplementedException();
        }

        protected void Add()
        {
            StyleClass.WriteDivider("Title");
            var title = AnsiConsole.Ask<string>("Write [green]Title[/]:");

            StyleClass.WriteDivider("Value");
            var value = AnsiConsole.Ask<decimal>("Enter the [green]Value[/]:");

            StyleClass.WriteDivider("Date");
            var date = AnsiConsole.Prompt(
                new TextPrompt<DateOnly>("Enter the [green]Date[/] in default is today date [grey]yyyy.MM.dd[/]:")
                    .PromptStyle("green")
                    .ValidationErrorMessage("[red]Please use this format yyyy.MM.dd[/]")
                    .DefaultValue(DateOnly.FromDateTime(DateTime.Today)));
            StyleClass.WriteDivider("string");
            var description = AnsiConsole.Ask<string>("Tell more [green]Description[/]");
            _controller.Add(Program.loggedUserID, title, value, date, description, SelectedCategories());
        }
        private void Delete()
        {
            var idToDelete = AnsiConsole.Ask<string>("Write the ID of the items you want to remove.\n e.g. 1-5, 8, 11-13");
            var ids = ParseIdRanges(idToDelete);
        }
        private void ViewAll(int? value)
        {
            int maxLength;
            if (value is null)
            {
                maxLength = int.MaxValue;
            }
            else 
            {
                maxLength = (int)value;
            }
            var table = new Table().Centered();
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
                    foreach (Income i in _controller.GetAll(Program.loggedUserID))
                    {
                        var categories = _controller._categoryRepo.GetByIncomeID(i.Id);
                        var categoriesString = String.Join(", ", categories);
                        table.AddRow(
                            $"[{Color.Purple}]{i.Id}[/]",
                            $"[{Color.Yellow}]{SafeSubstring(i.Title, 0, maxLength)}[/]",
                            $"[{Color.Lime}]{i.Value}[/]",
                            $"[{Color.DarkMagenta}]{i.date}[/]",
                            $"[{Color.Aqua}]{SafeSubstring(categoriesString,0,maxLength)}[/]",
                            $"[{Color.Silver}]{SafeSubstring(i.Description, 0, maxLength)}[/]");
                        context.Refresh();
                    }
                });
        }
        private void ViewAllByFiltr()
        {

        }
        private void SortIncomes()
        {
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
    }
}
