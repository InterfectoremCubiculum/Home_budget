namespace Home_budget.Views
{
    using Home_budget_library.Controllers;
    using Spectre.Console;
    using System.Text;

    public static class Program
    {

        public static readonly Panel header = StyleClass.HEADER_1;
        public static int loggedUserID = 0;
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.InputEncoding = Encoding.UTF8;

            Layout layout = new Layout();

            layout.SplitRows(
                    new Layout("Header").Ratio(2),
                    new Layout("Body").Ratio(4)
                    .SplitRows(
                        new Layout("Top")
                        .SplitColumns(new Layout("LeftTop"), new Layout("RightTop")
                            ),
                        new Layout("Bottom")
                        .SplitColumns(new Layout("LeftBottom"), new Layout("RightBottom")
                            )
                        )
                    );
            layout["Header"].Update(header);
            layout["LeftTop"].Update(StyleClass.AddMenuPanel("Manage Transactions\n--->(1)<---[tan]\n[/]").Expand());
            layout["RightTop"].Update(StyleClass.AddMenuPanel("See Statistics\n--->(2)<---").Expand());
            layout["LeftBottom"].Update(StyleClass.AddMenuPanel("Log Out\n--->(3)<---").Expand());
            layout["RightBottom"].Update(StyleClass.AddMenuPanel("Exit\n--->(ESC)<---").Expand());


            ConsoleKey key;
            AnsiConsole.Cursor.Hide();

            while (true)
            {
                if (loggedUserID == 0)
                {
                    // Create controller
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
                        case ConsoleKey.Escape:
                            Environment.Exit(0);
                            return;
                    }
                }
            }
        }
    }
}
