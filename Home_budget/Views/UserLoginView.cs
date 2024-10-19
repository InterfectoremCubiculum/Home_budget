using Home_budget_library.Controllers;
using Spectre.Console;

namespace Home_budget.Views
{
    public class UserLoginView
    {
        private readonly UserController _controller;

        private int login_attempts = 0;
        private readonly static int MAX_ATTEMPS = 3;

        //Panels for UserLoginView
        private readonly Panel header;
        private readonly Panel instruction;

        public UserLoginView(UserController controller)
        {
            _controller = controller;
            header = StyleClass.HEADER_2;

            instruction = new Panel(new Markup(
                    " 1. Minimal password lenght is 8 characters\n" +
                    " 2. Minimal username lenght is 6 characters"
                    )).Expand().Header("Instruction")
                    .BorderColor(StyleClass.BORDER_COLOR);
        }
        public void OnStart()
        {
            AnsiConsole.Clear();
            //AnsiConsole.Write(header);
            ShowLoginMenu();
        }
        private void ShowLoginMenu()
        {
            AnsiConsole.Cursor.Hide();
            //Panel empty = new Panel("")
            Layout layout = new Layout();

            layout.SplitRows(
                    new Layout("Header").Ratio(11),
                    new Layout("Body").Ratio(12)
                        .SplitColumns(
                        new Layout("Left").Ratio(5),
                        new Layout("Mid").Ratio(3)
                        .SplitRows(
                            new Layout("Top"),
                            new Layout("Center"),
                            new Layout("Bottom")
                            ),
                        new Layout("Right").Ratio(5)
                        ));
            layout["Header"].Update(header);
            layout["Top"].Update(
                StyleClass.AddMenuPanel(" Login\n--->(1)<---").Expand()
                ).Size(4);
            layout["Center"].Update(
                StyleClass.AddMenuPanel(" Create Account\n--->(2)<---").Expand()
                ).Size(4);
            layout["Bottom"].Update(
                StyleClass.AddMenuPanel(" Exit\n--->(ESC)<---").Expand()
                ).Size(4);
            layout["Left"].Update(
                new Text("")
            );
            layout["Right"].Update(
              new Text("")
            );
            AnsiConsole.Write(layout);

            ConsoleKey key;

            do
            {
                key = Console.ReadKey(true).Key;
                switch (key)
                {
                    case ConsoleKey.D1:
                        LoginAttempt();
                        break;
                    case ConsoleKey.D2:
                        CreateAccount();
                        break;
                    case ConsoleKey.Escape:
                        Environment.Exit(0);
                        return;
                }
            }
            while (key != ConsoleKey.Escape);
            /*
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
                                Environment.Exit(0);
                                return;
                        }*/
        }
        private void LoginAttempt()
        {

            login_attempts++;

            // Ask for username
            StyleClass.WriteDivider("Username");
            var username = AnsiConsole.Ask<string>("Enter your [green]username[/]:");

            // Ask for password
            StyleClass.WriteDivider("Password");
            var password = AnsiConsole.Prompt(
                new TextPrompt<string>("Enter your [green]password[/]:")
                    .PromptStyle("red")
                    .Secret());

            // Validate user credentials
            var isValid = _controller.ValidateLogin(username, password);

            if (isValid)
            {
                GoToMainMenu(username);
                AnsiConsole.Clear();
                AnsiConsole.Write(new FigletText("Welcome")
                    .Centered()
                    .Color(Color.Green));
                AnsiConsole.MarkupLine("[green]Login successful![/]\nPress any key to go futher");
                Console.ReadKey(true);
            }
            else
            {
                if (login_attempts < MAX_ATTEMPS)
                {
                    AnsiConsole.Clear();
                    AnsiConsole.Write(header);
                    AnsiConsole.MarkupLine($"\n[red]Login failed![/] Try Again, attemps remain: [rapidblink]{MAX_ATTEMPS - login_attempts}[/]");
                    LoginAttempt();
                }
                else
                {
                    AnsiConsole.MarkupLine("[red]Too many attemps.[/] Press any key to close the terminal...");
                    Console.ReadKey(true);
                    Environment.Exit(0);
                }
            }
        }
        private void CreateAccount()
        {
            AnsiConsole.Write(instruction);
            StyleClass.WriteDivider("Username");
            string username = AskUsername();

            Panel controllPanel = StyleClass.AddPanel($"Your username [green]{username}[/]");
            StyleClass.ClearWrite([header, instruction, controllPanel]);

            StyleClass.WriteDivider("Password");
            string password = AskPassword();
            controllPanel = StyleClass.AddPanel($"Your username: [green]{username}[/] \nYour password [green][/] :check_mark:");
            StyleClass.ClearWrite([header, instruction, controllPanel]);


            StyleClass.WriteDivider("RepatePassword");
            AskSecondPassword(password);

            if (_controller.AddUser(username, password))
            {
                GoToMainMenu(username);
                AnsiConsole.Clear();
                AnsiConsole.Write(header);
                AnsiConsole.Markup("[Green][Blink]Your account was successfully made[/][/]\nPress any key to go futher");
                Console.ReadKey(true);
            }
            else
            {
                AnsiConsole.Clear();
                AnsiConsole.Write(header);
                AnsiConsole.Markup("[Green][Blink]Error encountered[/][/]\nPress any key to close the terminal...");
                Console.ReadKey(true);
            }


        }

        private string AskPassword()
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
        private string AskSecondPassword(string password)
        {
            return AnsiConsole.Prompt(
                new TextPrompt<string>("Enter same [green]password[/]:")
                    .Secret()
                    .PromptStyle("green")
                    .ValidationErrorMessage("[red]Password is incorrect[/]")
                    .Validate(password2 =>
                    {
                        var correctPassword = password.Equals(password2);
                        return correctPassword ? ValidationResult.Success() : ValidationResult.Error("[red]Password is not the same[/]");
                    }));
        }

        private string AskUsername()
        {
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
                                return ValidationResult.Error("[red]Username needs to have more than 5 characters[/]");
                            case 2:
                                return ValidationResult.Error("[red]Username is not available[/]");
                            case 3:
                                return ValidationResult.Error("[red]Username needs to have more than 5 characters[/]\n[red]and also is not available[/]");
                            default:
                                return ValidationResult.Error("[red]Unknown error[/]");
                        }
                    }));

        }

        private void GoToMainMenu(string username)
        {
            Program.loggedUserID = _controller.GetUserID(username);
        }
    }
}
