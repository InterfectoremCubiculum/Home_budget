using Home_budget_library.Models;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Home_budget.Views
{
    public class TransactionView
    {
        protected List<string>  ChoiceCategories(List <string> ListNames)
        {
            var category = AnsiConsole.Prompt(
            new MultiSelectionPrompt<string>()
            .Title("Choice categories")
            .NotRequired()
            .PageSize(10)
            .MoreChoicesText("[grey](Move up and down to reveal more categories)[/]")
            .InstructionsText(
                "[grey](Press [blue]<space>[/] to toggle a category, " +
                "[green]<enter>[/] to accept)[/]")
            .AddChoices(ListNames)
            );
            return category;
        }

        protected void Menu()
        {
            var option = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Please select an option")
                    .PageSize(7)
                    .AddChoices(new[] { "Add", "Delete", "Copy and Edit", "Edit", "View All", "Search", "Main Menu" }));

            switch (option)
            {
                case "Add":
                    break;
                case "Create":
                    break;
                case "Copy and Edit":
                    break;
                case "Edit":
                    break;
                case "View All":
                    break;
                case "Search":
                    break;
                case "Main Menu":
                    return;
            }
        }
    }
}
