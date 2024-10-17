using Home_budget_library.Controllers;
using Home_budget_library.Models;
using Microsoft.Identity.Client;
using Spectre.Console;

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
            while(true)
            {
                Menu();
            }
        }

        protected new void Menu() 
        {
            base.Menu();
        }
        private void Add()
        {
            StyleClass.WriteDivider("Title");
            var title = AnsiConsole.Ask<string>("Write [green]Title[/]:");

            StyleClass.WriteDivider("Value");
            var value = AnsiConsole.Ask<int>("Enter the [green]Value[/]:");

            StyleClass.WriteDivider("Date");
            var date = AnsiConsole.Ask<DateOnly>("Enter the [green]Date[/] in default is today date [grey]yyyy.mm.dd[/]:");

            StyleClass.WriteDivider("string");
            var description = AnsiConsole.Ask<DateOnly>("Tell more [green]Description[/]");
            _controller.Add(Program.loggedUserID,title, value, description, date, ChoiceCategories());
        }
        private void Delete()
        {

        }
        private void ViewAll()
        {

        }
        private void ViewAllByFiltr()
        {

        }
        private void SortIncomes()
        {

        }

        protected List<string> ChoiceCategories()
        {
            return base.ChoiceCategories(_controller._categoryRepo.GetAllNames());
        }
    }
}
