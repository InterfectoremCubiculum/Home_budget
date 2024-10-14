using Home_budget_library.Controllers;
using Spectre.Console;
using System.Diagnostics.Metrics;

namespace Home_budget.Views
{
    public class UserLoginView
    {
        private readonly UserController _controller;
        private int login_attempts = 0;
        private readonly static int MAX_ATTEMPS = 3;
        public UserLoginView(UserController controller)
        {
            _controller = controller;
        }

        public void Show()
        {
            /* AnsiConsole.Clear();
             var layout = CreateLayout();
             AnsiConsole.Write(layout);
             AskPassword();*/
            // Display the title
            Panel header = new Panel(
                new FigletText("Home Budget\nLogin")
                .Centered()
                .Color(Color.Aqua)
                );
            AnsiConsole.Write(header);
            /*   AnsiConsole.Write(new FigletText("Login")
                .Centered()
                .Color(Color.Aqua));*/
            ShowLoginMenu();
        }
        public void ShowLoginMenu()
        {

            var option = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Please select an option")
                    .PageSize(3)
                    .AddChoices(new[] { "Login", "Create Account", "Exit" }));

            switch (option)
            {
                case "Login":
                    LoginAttempt();
                    break;
                case "Create Account":
                    CreateAccount();
                    break;
                case "Exit":
                    return;
            }
        }
        public void LoginAttempt()
        {

            login_attempts++;

            // Ask for username
            WriteDivider("Username");
            var username = AnsiConsole.Ask<string>("Enter your [green]username[/]:");

            // Ask for password
            WriteDivider("Password");
            var password = AnsiConsole.Prompt(
                new TextPrompt<string>("Enter your [green]password[/]:")
                    .PromptStyle("red")
                    .Secret());

            // Validate user credentials
            var isValid = _controller.ValidateLogin(username, password);

            if (isValid)
            {
                AnsiConsole.Clear();
                AnsiConsole.Write(new FigletText("Welcome")
                    .Centered()
                    .Color(Color.Green));
                AnsiConsole.MarkupLine("[green]Login successful![/]");
            }
            else
            {
                if (login_attempts < MAX_ATTEMPS)
                {
                    AnsiConsole.MarkupLine($"[red]Login failed![/] Try Again, attemps remain: {MAX_ATTEMPS - login_attempts} \n");
                    LoginAttempt();
                }
                else
                {
                    AnsiConsole.MarkupLine("[grey]Press any key to close the terminal...[/]");
                    Console.ReadKey(true);
                    Environment.Exit(0);
                }
            }
        }
        public void CreateAccount()
        {
            Paragraph sda= new Paragraph();
            WriteDivider("Username");
            AskUsername();
            WriteDivider("Password");
            AskPassword();
        }

        public string AskPassword()
        {
            return AnsiConsole.Prompt(
                new TextPrompt<string>("Enter your [green]password[/]:")
                    .Secret()
                    .PromptStyle("green")
                    .ValidationErrorMessage("[red]Password is incorrect[/]")
                    .Validate(password =>
                    {
                        var correctPassword = _controller.Create_User_Check_Password(password);
                        return correctPassword ? ValidationResult.Success() : ValidationResult.Error("[red]Incorrect password[/]");
                    }));
        }

        public string AskUsername()
        {
            int count = 0;
            return AnsiConsole.Prompt(
                new TextPrompt<string>("Enter your [green]username[/]:")
                    .PromptStyle("green")
                    .ValidationErrorMessage("[red]Username is incorrect[/]")
                    .Validate(username =>
                    {
                        var checkingValue = _controller.Create_User_Check_UserName(username);
                        switch (checkingValue)
                        {
                            case 0:
                                return ValidationResult.Success();
                            case 1:
                                count++;
                                return ValidationResult.Error("[red]Username needs to have more than 5 characters[/]");
                            case 2:
                                count++;
                                return ValidationResult.Error("[red]Username is not available[/]");
                            case 3:
                                count++;
                                return ValidationResult.Error("[red]Username needs to have more than 5 characters[/]\n[red]and also is not available[/]");
                            default:
                                count++;
                                return ValidationResult.Error("[red]Unknown error[/]");
                        }
                    }));

        }
        private static void WriteDivider(string text)
        {
            AnsiConsole.WriteLine();
            AnsiConsole.Write(new Rule($"[yellow]{text}[/]").RuleStyle("grey").LeftJustified());
        }
    }
}
