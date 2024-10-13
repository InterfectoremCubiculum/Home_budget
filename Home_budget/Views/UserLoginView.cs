using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Home_budget_library.Controllers;

namespace Home_budget.Views
{
    public class UserLoginView
    {
        private readonly UserController _controller;

        public UserLoginView(UserController controller)
        {
            _controller = controller;
        }

        public void Show()
        {
            // Display the title
            AnsiConsole.Write(new FigletText("Login")
                .Centered()
                .Color(Color.Aqua));

            // Ask for username
            var username = AnsiConsole.Ask<string>("Enter your [green]username[/]:");

            // Ask for password
            var password = AnsiConsole.Prompt(
                new TextPrompt<string>("Enter your [green]password[/]:")
                    .PromptStyle("red")
                    .Secret());

            // Validate user credentials
            var isValid = _controller.ValidateLogin(username, password);

            if (isValid)
            {
                AnsiConsole.MarkupLine("[green]Login successful![/]");
            }
            else
            {
                AnsiConsole.MarkupLine("[red]Login failed![/]");
            }
        }
    }
}
