using MaterialDesignThemes.Wpf;
using System.Globalization;
using System.Windows;
using System.Windows.Markup;

namespace Home_budget_graphic
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        internal BaseTheme InitialTheme { get; set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            // Ustawienie kultury na en-GB dla całej aplikacji
            CultureInfo culture = new CultureInfo("pl");
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;

            // Ustawienie kultury dla wszystkich elementów WPF
            FrameworkElement.LanguageProperty.OverrideMetadata(
                typeof(FrameworkElement),
                new FrameworkPropertyMetadata(
                    XmlLanguage.GetLanguage(culture.IetfLanguageTag)));

            base.OnStartup(e);
        }
    }

}
