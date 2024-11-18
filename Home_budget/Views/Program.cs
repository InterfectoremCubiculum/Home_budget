namespace Home_budget.Views
{
    using Home_budget_library.Controllers;
    using Spectre.Console;
    using System;
    using System.Diagnostics;
    using System.Text;

    public static class Program
    {
        public static readonly Panel header = StyleClass.HEADER_1;
        private static int loggedUserID = 0;

        public static int LoggedUserID { get => loggedUserID; set => loggedUserID = value; }

        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.InputEncoding = Encoding.UTF8;

            AnsiConsole.WriteLine("Wybierz tryb aplikacji:");
            AnsiConsole.WriteLine("1. Tryb konsolowy");
            AnsiConsole.WriteLine("2. Tryb graficzny (WPF)");
            AnsiConsole.WriteLine("ESC. Exit");
            ConsoleKey key;
            AnsiConsole.Cursor.Hide();
            while (true)
            {
                key = Console.ReadKey(true).Key;
                switch (key)
                {
                    case ConsoleKey.D1:
                        RunConsoleApp();
                        break;
                    case ConsoleKey.D2:
                        RunWpfApp();
                        Environment.Exit(0);
                        break;
                    case ConsoleKey.Escape:
                        Environment.Exit(0);
                        break;
                    default:
                        AnsiConsole.WriteLine("Nieprawidłowy wybór.");
                        break;
                }

            }
        }

        static void RunConsoleApp()
        {

            Layout layout = new();

            layout.SplitRows(
                    new Layout("Header").Ratio(2),
                    new Layout("Body").Ratio(4)
                    .SplitRows(
                        new Layout("Top")
                        .SplitColumns(new Layout("LeftTop"), new Layout("RightTop")
                            ),
                        new Layout("Bottom")
                        .SplitColumns(new Layout("LeftBottom"),new Layout("Middle") ,new Layout("RightBottom")
                            )
                        )
                    );
            layout["Header"].Update(header);
            layout["LeftTop"].Update(StyleClass.AddMenuPanel("Manage Transactions\n--->(1)<---[tan]\n[/]").Expand());
            layout["RightTop"].Update(StyleClass.AddMenuPanel("See Statistics\n--->(2)<---").Expand());
            layout["LeftBottom"].Update(StyleClass.AddMenuPanel("Log Out\n--->(3)<---").Expand());
            layout["RightBottom"].Update(StyleClass.AddMenuPanel("Exit\n--->(ESC)<---").Expand());
            layout["Middle"].Update(StyleClass.AddMenuPanel("Change to Graphic Mode \n--->(4)<---").Expand());

            ConsoleKey key;
            AnsiConsole.Cursor.Hide();

            while (true)
            {
                if (loggedUserID == 0)
                {
                    var userController = new UserController();
                    var loginView = new UserLoginView(userController);
                    loginView.OnStart();
                }
                else
                {
                    AnsiConsole.Cursor.Hide();
                    AnsiConsole.Write(layout);
                    key = Console.ReadKey(true).Key;
                    switch (key)
                    {
                        case ConsoleKey.D1:
                            AnsiConsole.Clear();
                            new TransactionView(new TransactionController()).OnStart();
                            break;
                        case ConsoleKey.D2:
                            AnsiConsole.Clear();
                            new StatisticsView(new StatisticsController()).OnStart();
                            break;
                        case ConsoleKey.D3:
                            AnsiConsole.Clear();
                            AnsiConsole.Cursor.Show();
                            loggedUserID = 0;
                            break;
                        case ConsoleKey.D4:
                            RunWpfApp();
                            Environment.Exit(0);
                            break;
                        case ConsoleKey.Escape:
                            Environment.Exit(0);
                            return;
                        default:
                            AnsiConsole.Clear();
                            break;
                    }
                }
            }
        }

        static void RunWpfApp()
        {
            try
            {
                var startInfo = new ProcessStartInfo
                {
                    FileName = "Home_budget_graphic.exe",
                    UseShellExecute = true
                };
                Process.Start(startInfo);
            }
            catch (Exception ex)
            {
                Console.WriteLine("WPF not found or indifferent dictionary " + ex.Message);
                return;
            }
        }
    }
}
