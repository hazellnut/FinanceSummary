using FinanceSummary.ViewModels;
using FinanceSummary.Views;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using FinanceSummary.Views;

namespace FinanceSummary
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        public MainWindow window;

        public TransactionView transactions = new();
        public TransactionView_ViewModel transactions_viewmodel = new();
        protected override void OnStartup(StartupEventArgs e)
        {
            window = new MainWindow(transactions, transactions_viewmodel);
            window.Show();

            base.OnStartup(e);
        }

    }
}
