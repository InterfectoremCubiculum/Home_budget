using Home_budget_library.Controllers;
using Spectre.Console;
using System.Drawing;

namespace Home_budget.Views
{
    public class UserLoginView
    {
        
        private readonly UserController _controller;

        
        private int login_attempts = 0; //Count all login attempts 
        private readonly static int MAX_ATTEMPS = 3; //Max number of attemps to log

        //Panels for UserLoginView
        private readonly Panel header;
        private readonly Panel instruction;
      
        /// <summary>
        /// Is creating view panels and saving userController taken from parameter
        /// </summary>
        /// <param name="controller">It is controller which will communicate with view</param>
        public UserLoginView(UserController controller)
        {
            _controller = controller;

            header = StyleClass.HEADER_2;
            instruction = new Panel(new Markup(
                    $"[{StyleClass.T_COL_STR}] 1. Minimal password lenght is 8 characters\n" +
                    " 2. Minimal username lenght is 6 characters[/]"
                    )).Expand().Header("Instruction")
                    .BorderColor(StyleClass.BORDER_COLOR)
                    .RoundedBorder();
        }

        /// <summary>
        /// Clears the console and displays the menu
        /// </summary>
        public void OnStart()
        {
            AnsiConsole.Clear();
            ShowLoginMenu();
        }

        /// <summary>
        /// Creates a view layout of menu and listens for buttons.
        /// Operate selected buttons to navigate through the menu
        /// </summary>
       
        private void ShowLoginMenu()
        {
            AnsiConsole.Cursor.Hide();
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
                        AnsiConsole.Clear();
                        AnsiConsole.Write(header);
                        AnsiConsole.Cursor.Show();
                        LoginAttempt();
                        return;
                    case ConsoleKey.D2:
                        AnsiConsole.Clear();
                        AnsiConsole.Write(header);
                        AnsiConsole.Cursor.Show();
                        CreateAccount();
                        return;
                    case ConsoleKey.Escape:
                        Environment.Exit(0);
                        return;
                }
            }
            while (key != ConsoleKey.Escape);
        }

        /// <summary>
        /// Is responsible for logging the user, 
        /// Counts the number of attempts and interacts with the user.
        /// </summary>
        private void LoginAttempt()
        {
            login_attempts++;

            // Ask for username
            StyleClass.WriteDivider("Username");
            
            var username = AnsiConsole.Ask<string>($"[{StyleClass.T_COL_STR}]Enter your [{StyleClass.T_HL}]username[/]:[/]");

            // Ask for password
            StyleClass.WriteDivider("Password");
            var password = AnsiConsole.Prompt(
                new TextPrompt<string>($"[{StyleClass.T_COL_STR}]Enter your [{StyleClass.T_HL}]password[/]:[/]")
                    .PromptStyle($"{StyleClass.T_HL_ERR_STR}")
                    .Secret());

            // Validate user credentials
            var isValid = _controller.ValidateLogin(username, password);

            if (isValid)
            {
                GoToMainMenu(username);
                //AnsiConsole.Clear();
                //AnsiConsole.Write(new FigletText("Welcome")
                //    .Centered()
                //    .Color(Color.Green));
            }
            else
            {
                if (login_attempts < MAX_ATTEMPS)
                {
                    AnsiConsole.Clear();
                    AnsiConsole.Write(header);
                    AnsiConsole.MarkupLine($"\n[{StyleClass.T_HL_ERR_STR}]Login failed![/] Try Again, attemps remain: [rapidblink]{MAX_ATTEMPS - login_attempts}[/]");
                    LoginAttempt();
                }
                else
                {
                    AnsiConsole.Clear();
                    AnsiConsole.Write(header);
                    AnsiConsole.MarkupLine($"\n[{StyleClass.T_HL_ERR_STR}]Too many attemps.[/] Press any key to close the terminal...");
                    Console.ReadKey(true);
                    Environment.Exit(0);
                }
            }
        }


        /// <summary>
        /// Manages the creation of a new account
        /// </summary>
        private void CreateAccount()
        {
            // Show instruction 
            AnsiConsole.Write(instruction);

            // Ask for username
            StyleClass.WriteDivider("Username");
            string username = AskUsername();

            Panel controllPanel = StyleClass.AddPanel($"[{StyleClass.T_COL_STR}]Your username[/] [{StyleClass.T_HL}]{username}[/]");
            StyleClass.ClearWrite([header, instruction, controllPanel]);

            // Ask for password
            StyleClass.WriteDivider("Password");
            string password = AskPassword();
            controllPanel = StyleClass.AddPanel($"[{StyleClass.T_COL_STR}]Your username[/] [{StyleClass.T_HL}]{username}[/] \n[{StyleClass.T_COL_STR}]Your password[/] [{StyleClass.T_HL}][/] :check_mark:");
            StyleClass.ClearWrite([header, instruction, controllPanel]);

            // Ask for repeat password
            StyleClass.WriteDivider("Repeat Password");
            AskSecondPassword(password);

            //Iinforms about successful or unsuccessful account creation
            if (_controller.AddUser(username, password))
            {
                GoToMainMenu(username);
                AnsiConsole.Clear();
                //AnsiConsole.Write(header);
                //AnsiConsole.Markup("[darkolivegreen1]\n[Blink]Your account was successfully made[/][/]\nPress any key to go futher");
                //Console.ReadKey(true);
            }
            else
            {
                AnsiConsole.Clear();
                AnsiConsole.Write(header);
                AnsiConsole.Markup($"[{StyleClass.T_HL}]\n[Blink]Error encountered[/][/]\nPress any key to close the terminal...");
                Console.ReadKey(true);
            }


        }


        /// <summary>
        /// This a prompt that validate input date. Handle password entry
        /// </summary>
        /// <returns>
        /// Its return password if all is correct, if not it ask for password
        /// </returns>
        private string AskPassword()
        {
            return AnsiConsole.Prompt(
                new TextPrompt<string>($"[{StyleClass.T_COL_STR}]Enter your[/] [{StyleClass.T_HL}]password[/]:")
                    .Secret()
                    .ValidationErrorMessage($"[{StyleClass.T_HL_ERR_STR}]Password is incorrect[/]")
                    .Validate(password =>
                    {
                        var correctPassword = _controller.Create_User_Check_Password(password);
                        return correctPassword ? ValidationResult.Success() : ValidationResult.Error($"[{StyleClass.T_HL_ERR_STR}]Incorrect password[/]");
                    }));
        }

        /// <summary>
        /// This a prompt that validate input date. Handle repeat password entry
        /// </summary>
        /// <returns>
        /// Its return password if all is correct, if not it ask for password
        /// </returns>
        private string AskSecondPassword(string password)
        {
            return AnsiConsole.Prompt(
                new TextPrompt<string>($"[{StyleClass.T_COL_STR}]Enter same[/] [{StyleClass.T_HL}]password[/]:")
                    .Secret()
                    .ValidationErrorMessage($"[{StyleClass.T_HL_ERR_STR}]Password is incorrect[/]")
                    .Validate(password2 =>
                    {
                        var correctPassword = password.Equals(password2);
                        return correctPassword ? ValidationResult.Success() : ValidationResult.Error($"[{StyleClass.T_HL_ERR_STR}]Password is not the same[/]");
                    }));
        }

        /// <summary>
        /// This a prompt that validate input date. Handle username entry
        /// </summary>
        /// <returns>
        /// Its return username if all is correct, if not it ask for username
        /// </returns>
        private string AskUsername()
        {
            return AnsiConsole.Prompt(
                new TextPrompt<string>($"[{StyleClass.T_COL_STR}]Enter your[/] [{StyleClass.T_HL}]username[/]:")
                    .ValidationErrorMessage($"[{StyleClass.T_HL_ERR_STR}]Username is incorrect[/]")
                    .Validate(username =>
                    {
                        var checkingValue = _controller.Create_User_Check_UserName(username);
                        switch (checkingValue)
                        {
                            case 0:
                                return ValidationResult.Success();
                            case 1:
                                return ValidationResult.Error($"[{StyleClass.T_HL_ERR_STR}]Username needs to have more than 5 characters[/]");
                            case 2:
                                return ValidationResult.Error($"[{StyleClass.T_HL_ERR_STR}]Username is not available[/]");
                            case 3:
                                return ValidationResult.Error($"[{StyleClass.T_HL_ERR_STR}]Username needs to have more than 5 characters[/]\n[{StyleClass.T_HL_ERR_STR}]and also is not available[/]");
                            default:
                                return ValidationResult.Error($"[{StyleClass.T_HL_ERR_STR}]Unknown error[/]");
                        }
                    }));

        }

        private void GoToMainMenu(string username)
        {
            Program.loggedUserID = _controller.GetUserID(username);
        }
    }
}
