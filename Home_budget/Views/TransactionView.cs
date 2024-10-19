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
        protected Dictionary<int, string> SelectedCategories(Dictionary <int, string> categoriesDictionary)
        {
            var selectedNames = AnsiConsole.Prompt(
            new MultiSelectionPrompt<string>()
            .Title("Choice categories")
            .NotRequired()
            .PageSize(10)
            .MoreChoicesText("[grey](Move up and down to reveal more categories)[/]")
            .InstructionsText(
                "[grey](Press [blue]<space>[/] to toggle a category, " +
                "[green]<enter>[/] to accept)[/]")
            .AddChoices(categoriesDictionary.Values)
            );
            var selectedCategories = categoriesDictionary
                .Where(pair => selectedNames.Contains(pair.Value))
                .ToDictionary(pair => pair.Key, pair => pair.Value);

            return selectedCategories;
        }


        protected void Add() 
        {
        }
    }
}
