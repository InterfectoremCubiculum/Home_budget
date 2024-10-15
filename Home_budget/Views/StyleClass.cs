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
    }
}
