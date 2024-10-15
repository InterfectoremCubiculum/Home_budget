namespace Home_budget.Views
{
    using Home_budget_library.Controllers;
    using Home_budget_library.Models;
    using Spectre.Console;
    using System.Text;

    public static class Program
    {
        // -1 If not logged
        public static int loggedUserID = -1;
        static void Main(string[] args)
        {


            Console.OutputEncoding = Encoding.UTF8;
            AnsiConsole.Write(
            new Panel("[yellow]Hello :globe_showing_europe_africa:![/]")
                .RoundedBorder());
            Thread.Sleep(7000);
            // Create controller
            var userController = new UserController();

            // Display the login view
            var loginView = new UserLoginView(userController);
            loginView.Show();
        }
    }
}
