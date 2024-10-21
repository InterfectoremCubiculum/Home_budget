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
        public static readonly Style TEXT_COLOR = new Style(new Color(242, 214, 162));
        public static readonly Color BORDER_COLOR = new Color(242, 170, 107);
        public static readonly Color FIGLETTEXT_COLOR = new Color(242, 198, 153);
        public static readonly Color DIVIDER_COLOR = new Color(242, 170, 107);

        public static readonly string T_HL_ERR_STR = "rgb(242,85,91)";
        public static readonly string T_COL_STR = "rgb(242,214,162)";
        public static readonly string FIGL_COL_STR = "rgb(242,198,153)";
        public static readonly string T_HL_STR =  "rgb(242,255,127)";
        public static readonly Panel HEADER_1 = new Panel(
               new FigletText("Home Budget")
               .Centered()
               .Color(FIGLETTEXT_COLOR)
               ).BorderColor(BORDER_COLOR)
                .RoundedBorder()
                .Expand();
        public static readonly Panel HEADER_2 = new Panel(
               new FigletText("Home Budget\nLogin")
               .Centered()
               .Color(FIGLETTEXT_COLOR)
               ).BorderColor(BORDER_COLOR)
            .RoundedBorder();


        public static Panel AddPanel(string message)
        {
            Panel panel = new Panel(
                new Markup(message)
              
               ).BorderColor(BORDER_COLOR)
                .RoundedBorder();

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
            AnsiConsole.Write(new Rule($"[{FIGL_COL_STR}]{text}[/]").RuleStyle(DIVIDER_COLOR).LeftJustified());
        }

        public static Panel AddMenuPanel(string text) 
        {
            Panel menuPanel = new Panel(Align.Center(new Markup(text, TEXT_COLOR)))
                        .BorderColor(StyleClass.BORDER_COLOR)
                        .RoundedBorder()
                        .Padding(1,0,1,0);
            return menuPanel;
        }

        public static T AskForInput<T>(string dividerText, string promptText, string highLightedText, T defaultValue = default)
        {
            WriteDivider(dividerText);
            return AnsiConsole.Prompt(
                new TextPrompt<T>($"[{T_COL_STR}]{promptText}[/] [{T_HL_STR}]{highLightedText}[/]:")
                    .DefaultValue(defaultValue)
            );
        }
        public static T AskForInput<T>(string dividerText, string promptText, string highLightedText)
        {
            WriteDivider(dividerText);
            return AnsiConsole.Prompt(
                new TextPrompt<T>($"[{T_COL_STR}]{promptText}[/] [{T_HL_STR}]{highLightedText}[/]:")
            );
        }
    }
}
