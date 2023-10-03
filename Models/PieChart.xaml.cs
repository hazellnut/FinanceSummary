using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FinanceSummary.Models
{
    /// <summary>
    /// Interaction logic for PieChart.xaml
    /// </summary>
    public partial class PieChart : UserControl
    {
        public PieChart()
        {
            InitializeComponent();
            Children = new ObservableCollection<PieShape>();
            DataContext = this;
        }

        public ObservableCollection<PieShape> Children { get; set; }
        public double radius { get; set; }

        public PieShape SelectedChild { get; set; }
        public void ArrangeChildren()
        {
            MainCanvas.Children.Clear();
            int childCount = Children.Count;
            double angleIncrement = 360.0 / childCount;

            for (int i = 0; i < childCount; i++)
            {
                MainCanvas.Children.Add(Children[i]);
                //    double angle = i * angleIncrement;
                //    double radians = angle * (Math.PI / 180.0);
                //    double x = MainCanvas.ActualWidth / 2;
                //    double y = MainCanvas.ActualHeight / 2;

                Canvas.SetLeft(Children[i], radius*1.5);
                Canvas.SetTop(Children[i], radius*1.5);
                //}
            }
        }
    }
}
