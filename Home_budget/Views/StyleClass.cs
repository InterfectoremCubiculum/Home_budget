using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Home_budget.Views
{
    public static class StyleClass
    {
        public static readonly Style TEXT_COLOR = new Style(new Color(4, 191, 191));
        public static readonly Color BORDER_COLOR = new Color(242, 107, 131);
        public static readonly Color FIGLETTEXT_COLOR = new Color(4, 191, 191);
        public static readonly Panel HEADER_1 = new Panel(
               new FigletText("Home Budget")
               .Centered()
               .Color(FIGLETTEXT_COLOR)
               ).BorderColor(BORDER_COLOR);
        public static readonly Panel HEADER_2 = new Panel(
               new FigletText("Home Budget\nLogin")
               .Centered()
               .Color(FIGLETTEXT_COLOR)
               ).BorderColor(BORDER_COLOR);


        public static Panel AddPanel(string message)
        {
            Panel panel = new Panel(
                new Markup(message)
               );
            return panel;
        }
        public static void Write(List<Panel> panels)
        {
            foreach (Panel panel in panels)
            {
                AnsiConsole.Write(panel);
            }
        }
        public static void ClearWrite(List<Panel> panels)
        {
            AnsiConsole.Clear();
            Write(panels);
        }
        public static void WriteDivider(string text)
        {
            AnsiConsole.WriteLine();
            AnsiConsole.Write(new Rule($"[yellow]{text}[/]").RuleStyle("grey").LeftJustified());
        }
        public static Panel AddMenuPanel(string text) 
        {
            Panel menuPanel = new Panel(new Markup(text, StyleClass.TEXT_COLOR).Centered())
                        .BorderColor(StyleClass.BORDER_COLOR)
                        .Padding(1,0,1,0);
            return menuPanel;
        }
    }
}
