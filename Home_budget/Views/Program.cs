namespace Home_budget.Views
{
    using Home_budget_library.Controllers;
    using Spectre.Console;
    using System.Text;

    public static class Program
    {
        // -1 If not logged
        public static bool loggedUserID = false;
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.InputEncoding = Encoding.UTF8;



            // Display the login view
            while (true)
            {
                if (loggedUserID == false)
                {
                    // Create controller
                    var userController = new UserController();
                    var loginView = new UserLoginView(userController);
                    loginView.Show();
                }
                else
                {
                    var option = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("Please select an option")
                        .PageSize(4)
                        .AddChoices(new[] { "Login", "Create Account", "Exit" }));
                    switch (option)
                    {
                        case "Manage your expenses":
                            new IncomeView();
                            break;
                        case "Manage your revenue":
                            new IncomeView();
                            break;
                        case "Log Out":
                            loggedUserID = false;
                            new IncomeView();
                            break;
                        case "Exit":
                            return;
                    }
                }
            }
        }
    }
}
