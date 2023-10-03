using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FinanceSummary.Models.Views
{
    /// <summary>
    /// Interaction logic for Summary.xaml
    /// </summary>
    public partial class Summary : UserControl
    {
        public Summary()
        {
            InitializeComponent();

            GetTransactions();
            
            if (selectedDates != null)
            {
                SummariseTransactions();
                GeneratePieChart();
            }
            //PieChart.ArrangeChildren();
        }
        public List<Transaction> Transactions { get; set; }
        public Dictionary<string, List<double>> Piechart_Values { get; set; } = new();
        public Dictionary<string, double> Summarised_Values { get; set; } = new();
        public SelectedDatesCollection selectedDates { get; set; }
        public void GetTransactions()
        {
            Transactions = DatabaseAccess.get_transactions().ToList();

        }


        public void SummariseTransactions()
        {
            GetTransactions();
            Summarised_Values.Clear();
            Piechart_Values.Clear();
            foreach (Transaction tr in Transactions)
            {
                if (!(tr.Category == "Income") && !(tr.Category == "Transfer") && !(tr.Category == "Uncategorised"))
                {
                    if (tr.datetime > selectedDates[0])
                    {
                        if (!Piechart_Values.ContainsKey(tr.Category))
                        {
                            Piechart_Values.Add(tr.Category, new List<double>());
                        }
                        if (tr.Amount > 0)
                        {
                            Piechart_Values[tr.Category].Add(tr.Amount);
                        }
                    }
                }
            }
            foreach (string tr in Piechart_Values.Keys)
            {
                
                Summarised_Values.Add(tr, Piechart_Values[tr].Sum());
            }
        }




        public void GeneratePieChart()
        {
            PieChart.Children.Clear();
            double total_amount = Summarised_Values.Values.Sum();
            double running_angle = 0;

            //total_amount = 100;
            //Summarised_Values.Clear();
            //Summarised_Values.Add("test1", 99.8);
            //Summarised_Values.Add("test2", 0.2);

            foreach (string tr in Summarised_Values.Keys)
            {
                int keyindex = Random.Shared.Next(1, brushes.Length);
                SolidColorBrush color = brushes[keyindex];
                PieShape ps = new();
                ps.colour = color;
                double pc_value = Summarised_Values[tr] / total_amount;
                ps.radius = PieChart.radius;
                ps.starting_angle = running_angle;
                ps.angle_width = pc_value * 360;
                running_angle += ps.angle_width;
                ps.detailed_text = tr + "\n Total amount: $" + Summarised_Values[tr].ToString("F1");
                ps.percentage = (pc_value*100).ToString("F1") + "%";
                PieChart.Children.Add(ps);
            }
            PieChart.ArrangeChildren();
        }

        public SolidColorBrush[] brushes
        {
            get {
                return GetSolidColorBrushes();
            }
        }
        public static SolidColorBrush[] GetSolidColorBrushes()
        {
            PropertyInfo[] properties = typeof(Brushes).GetProperties();

            var solidColorBrushes = properties
                .Where(prop => prop.PropertyType == typeof(SolidColorBrush))
                .Select(prop => prop.GetValue(null) as SolidColorBrush)
                .ToArray();

            return solidColorBrushes;
        }


        private double mean(List<double> values)
        {
            return (values.Sum()/ values.Count());
        }


        private void start_date_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedDates = start_date.SelectedDates;
            SummariseTransactions();
            GeneratePieChart();
        }
    }
}
