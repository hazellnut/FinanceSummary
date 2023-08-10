using FinanceSummary.ViewModels;
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

namespace FinanceSummary
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(TransactionView view, TransactionView_ViewModel vm)
        {
            InitializeComponent();

            view.DataContext = vm;

            TabItem tab = new()
            {
                Content = view,
                Header = "Transactions",
                Name = "Transactions"
            };
            Tabs.Items.Add(tab);
        }
    }
}
