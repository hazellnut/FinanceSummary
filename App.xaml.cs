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
using Microsoft.Extensions.Configuration;
using FinanceSummary.Models;

namespace FinanceSummary
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        public MainWindow window;
        
        public string connString { get; set; }
        public TransactionView transactions = new();
        public TransactionView_ViewModel transactions_viewmodel = new();
        protected override void OnStartup(StartupEventArgs e)
        {
            window = new MainWindow(transactions, transactions_viewmodel);
            window.Show();

            base.OnStartup(e);


            var config = new ConfigurationBuilder().AddUserSecrets<App>().Build();

            var secretProvider = config.Providers.First();
            secretProvider.TryGet("ConnectionString", out string retval);
            connString = retval;
            DatabaseAccess.ConnectionString = connString;
        }

    }
}
