using System;
using System.Collections.ObjectModel;
using System.Windows.Media;
using System.Windows.Controls;

namespace Home_budget_graphic.BarChart
{
    /// <summary>
    /// Logika interakcji dla klasy ChartControl.xaml
    /// </summary>
    public partial class ChartControl : UserControl
    {
        private double maxHeight = 200;
        private double maxValue = 1;

        public ObservableCollection<BarData> Bars { get; set; }
        public ChartControl()
        {
            InitializeComponent();
            Bars = new ObservableCollection<BarData>();
            DataContext = this;

        }
        public ChartControl AddBar(BarData barData) 
        {
            Bars.Add(barData);
            return this;
        }
        public ChartControl AddBar(double value, string label, Brush color )
        {
            Bars.Add(new BarData (value, label, color, maxHeight, maxValue));
            return this;
        }
        public ChartControl SetMaxHeigh(double height) 
        {
            maxHeight = height;
            return this;
        }
        public ChartControl SetMaxValue(double value)
        {
            maxValue = value;
            return this;
        }
        public ChartControl Clear()
        {
            Bars.Clear();
            return this;
        }
    }

    public class BarData
    {
        public double Value { get; set; }
        public string Label { get; set; }
        public Brush BarColor { get; set; }
        public double Height {  get; set; }
        public BarData(double value, string label, Brush color, double maxHeight, double maxValue) 
        { 
            this.Value = value;
            this.Label = label;
            this.BarColor = color;
            this.Height = Value > 0 ? Value / maxValue * maxHeight : 0;
        }
    }
}
