using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Home_budget_graphic.BarChart
{
    /// <summary>
    /// Logika interakcji dla klasy ChartBar.xaml
    /// </summary>
    public partial class ChartBar : UserControl
    {
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(double), typeof(ChartBar), new PropertyMetadata(0.0));

        public static readonly DependencyProperty LabelProperty =
            DependencyProperty.Register("Label", typeof(string), typeof(ChartBar), new PropertyMetadata(string.Empty));

        public static readonly DependencyProperty BarColorProperty =
            DependencyProperty.Register("BarColor", typeof(Brush), typeof(ChartBar), new PropertyMetadata(Brushes.Blue));

        public static readonly DependencyProperty HeightProperty = 
            DependencyProperty.Register("Height", typeof(double), typeof(ChartBar), new PropertyMetadata(0.0));

        public double Value
        {
            get => (double)GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        public string Label
        {
            get => (string)GetValue(LabelProperty);
            set => SetValue(LabelProperty, value);
        }

        public Brush BarColor
        {
            get => (Brush)GetValue(BarColorProperty);
            set => SetValue(BarColorProperty, value);
        }

        public double Height
        {
            get => (double)GetValue(HeightProperty);
            set => SetValue(HeightProperty, value);
        }

        public ChartBar()
        {
            InitializeComponent();
        }
    }

}
