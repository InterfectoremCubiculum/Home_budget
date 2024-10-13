namespace Home_budget
{
    using Home_budget.Views;
    using Spectre.Console;
    using Home_budget_library.Controllers;
    using System.Net.Mail;
    internal class Program
    {
        static void Main(string[] args)
        {
            // Initialize Spectre.Console
            AnsiConsole.Write(new FigletText("Home Budget")
                .Centered()
                .Color(Color.Aqua));

            // Create controller
            var userController = new UserController();

            // Display the login view
            var loginView = new UserLoginView(userController);
            loginView.Show();
        }
    }
}
