
using FinanceSummary.Views;
using System;
using System.Collections.Generic;
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
using FinanceSummary.Models.Views;
using FinanceSummary.Models;

namespace FinanceSummary
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Summary summary = new();
        public Summary_Viewmodel summary_vm = new();
        

        public Keywords keywords = new();
        public Keywords_ViewModel keywords_vm = new();

        public MainWindow(TransactionView view, TransactionView_ViewModel vm)
        {
            InitializeComponent();

            view.DataContext = vm;
            summary.DataContext = summary_vm;
            keywords.DataContext = keywords_vm;

            TabItem tab = new()
            {
                Content = view,
                Header = "Transactions",
                Name = "Transactions"
            };
            Tabs.Items.Add(tab);

            
            tab = new()
            {
                Content = summary,
                Header = "Summary",
                Name = "Summary"
            };

            Tabs.Items.Add(tab);

            tab = new()
            {
                Content = keywords,
                Header = "Keywords",
                Name = "Keywords"
            };
            Tabs.Items.Add(tab);
        }
    }
}
