﻿using Home_budget_library.Controllers;
using Home_budget_library.Models;
using Spectre.Console;
using System.Reflection.Metadata.Ecma335;

namespace Home_budget.Views
{
    public class TransactionView
    {
        private readonly TransactionController _controller;
        private readonly Panel navPanel;
        public TransactionView(TransactionController controller)
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
                        BaseView();
                        break;
                    case ConsoleKey.D2: //Delete
                        AnsiConsole.Cursor.Show();
                        Delete();
                        BaseView();
                        break;
                    case ConsoleKey.D3: //Copy and Edit
                        AnsiConsole.Cursor.Show();
                        Copy();
                        BaseView();
                        break;
                    case ConsoleKey.D4: //Edit
                        AnsiConsole.Cursor.Show();
                        EditView();
                        BaseView();
                        break;
                    case ConsoleKey.D5: //View All
                        AnsiConsole.Clear();
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
        protected void Add()
        {
            var title = StyleClass.AskForInput<string>("Title", "Write", "Title");
            var value = StyleClass.AskForInput<decimal>("Value", "Enter the", "Value");
            StyleClass.WriteDivider("Date");
            var date = AnsiConsole.Prompt(
                new TextPrompt<DateOnly>($"[{StyleClass.T_COL_STR}]Enter the [{StyleClass.T_HL_STR}]Date[/] in default is today date [grey]yyyy.MM.dd[/][/]:")
                    .PromptStyle("green")
                    .ValidationErrorMessage($"[{StyleClass.T_HL_ERR_STR}]Please use this format yyyy.MM.dd[/]")
                    .DefaultValue(DateOnly.FromDateTime(DateTime.Today)));
            var description = StyleClass.AskForInput<string>("Description", "Tell more", "Description");

            _controller.Add(Program.LoggedUserID, title, value, date, description, SelectedCategories(_controller.GetAllCategories()));
        }

        protected void Delete()
        {
            var idToDelete = AnsiConsole.Ask<string>($"[{StyleClass.T_COL_STR}]Write the ID of the items you want to remove. e.g. 1-5, 8, 11-13[/]:");
            var ids = ParseIdRanges(idToDelete);
            _controller.DeleteMany(ids, Program.LoggedUserID);
        }
        protected void Search()
        {
            var title = StyleClass.AskForInput<string>("Search by Title", "Write", "Title");
            var result = _controller.Search(title, Program.LoggedUserID);
            StyleClass.ClearWrite([navPanel]);
            WriteToTable(result, int.MaxValue);
            AnsiConsole.Write(navPanel);
        }
        protected void Copy()
        {
            var transactionId = StyleClass.AskForInput<int>("Id", "Enter the ID of the transaction you want to copy", "");
            var copiedTransaction = _controller.Copy(Program.LoggedUserID, transactionId);
            if (copiedTransaction == null)
                return;
            StyleClass.ClearWrite([navPanel]);
            AnsiConsole.Write(StyleClass.AddMenuPanel("Insert new data or click <ENTER> to leave it unchanged"));
            Edit(copiedTransaction);
        }
        protected void EditView()
        {
            var transactionId = StyleClass.AskForInput<int>("Id", "Enter the ID of the transaction you want to edit", "");
            AnsiConsole.Clear();

            var transaction = _controller.Get(Program.LoggedUserID, transactionId);
            if (transaction == null)
                return;
            AnsiConsole.Write(navPanel);
            AnsiConsole.Write(StyleClass.AddMenuPanel("Insert new data or click <ENTER> to leave it unchanged"));
            Edit(transaction);
        }

        private void Edit(Transaction transaction)
        {
            var newTitle = AnsiConsole.Prompt(new TextPrompt<string>($"[{StyleClass.T_COL_STR}]Enter the new [{StyleClass.T_HL_STR}]Title[/][/] ->")
                                            .DefaultValue(transaction.Title));
            var newValue = AnsiConsole.Prompt(new TextPrompt<decimal>($"[{StyleClass.T_COL_STR}]Enter the new [{StyleClass.T_HL_STR}]Value[/][/] ->")
                                            .DefaultValue(transaction.Value));
            var newDate = AnsiConsole.Prompt(new TextPrompt<DateOnly>($"[{StyleClass.T_COL_STR}]Enter the new [{StyleClass.T_HL_STR}]Date[/] [grey]yyyy.MM.dd[/][/] ->")
                                            .ValidationErrorMessage($"[{StyleClass.T_HL_ERR_STR}]Please use this format yyyy.MM.dd[/]")
                                            .DefaultValue(transaction.date));
            var newCategories = SelectedCategories(_controller.GetAllCategories());
            var newDescription = AnsiConsole.Prompt(new TextPrompt<string>($"[{StyleClass.T_COL_STR}]Enter the new [{StyleClass.T_HL_STR}]Description[/][/] ->")
                                            .DefaultValue(transaction.Description));
            _controller.Edit(transaction, newTitle, newValue, newDate, newDescription, newCategories);
        }

        protected void ViewAll(int? maxLength)
        {
            var listOfTransaction = _controller.GetAll(Program.LoggedUserID);
            maxLength ??= int.MaxValue;

            WriteToTable(listOfTransaction, (int)maxLength);
        }
        protected void WriteToTable(Dictionary<int, Transaction> listOfTransaction, int maxLength)
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

                    foreach (var item in listOfTransaction)
                    {
                        table.AddRow(
                            $"[{Color.Purple}]{item.Key}[/]",
                            $"[{Color.Yellow}]{SafeSubstring(item.Value.Title, 0, maxLength)}[/]",
                            $"[{Color.Lime}]{item.Value.Value}[/]",
                            $"[{Color.DarkMagenta}]{item.Value.date}[/]",
                            $"[{Color.Aqua}]{_controller.GetCategoryName(item.Value.CategoryID)}[/]",
                            $"[{Color.Silver}]{SafeSubstring(item.Value.Description, 0, maxLength)}[/]");
                        context.Refresh();
                    }
                });
        }
        protected int SelectedCategories( List<Category> categoriesList)
        {
            var selectedName = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Choice categories")
                .PageSize(10)
                .MoreChoicesText("[grey](Move up and down to reveal more categories)[/]")
                .AddChoices(categoriesList.Select(cat => cat.Name).ToList())
                );
            return _controller.GetCategoryId(selectedName);
        }

        private static string SafeSubstring(string str, int startIndex, int length)
        {
            if (startIndex < 0 || startIndex >= str.Length)
                return string.Empty;

            if (startIndex + length > str.Length)
            {
                length = str.Length - startIndex;
                return str.Substring(startIndex, length);
            }
            else
                return string.Concat(str.AsSpan(startIndex, length), "...");
        }

        private static List<int> ParseIdRanges(string input)
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
        protected void BaseView()
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(navPanel);
            ViewAll(35);
            AnsiConsole.Write(navPanel);
        }
        private static Panel CreateNavPanel()
        {
            return new Panel(
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
            ).BorderColor(StyleClass.BORDER_COLOR).RoundedBorder();
        }
    }
}
