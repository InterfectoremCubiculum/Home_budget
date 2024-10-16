namespace Home_budget.Views
{
    using Home_budget_library.Controllers;
    using Spectre.Console;
    using System.Text;

    public static class Program
    {
       static Panel header = new Panel(
               new FigletText("Home Budget")
               .Centered()
               .Color(Color.Aqua)
               );
        public static int loggedUserID = 0;
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.InputEncoding = Encoding.UTF8;



            // Display the login view
            while (true)
            {
                AnsiConsole.Clear();
                AnsiConsole.Write(header);
                if (loggedUserID == 0)
                {
                    // Create controller
                    var userController = new UserController();
                    var loginView = new UserLoginView(userController);
                    loginView.OnStart();
                }
                else
                {
                    var option = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("Please select an option")
                        .PageSize(4)
                        .AddChoices(new[] { "Manage your expenses", "Manage your revenue", "Log Out", "Exit" }));
                    switch (option)
                    {
                        case "Manage your expenses":
                            new IncomeView(new IncomeController());
                            break;
                        case "Manage your revenue":
                            new IncomeView(new IncomeController()).OnStart();
                            break;
                        case "Log Out":
                            loggedUserID = 0;
                            break;
                        case "Exit":
                            return;
                    }
                }
            }
        }
    }
}
