using FinanceSummary.Models;
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

namespace FinanceSummary.Views
{
    /// <summary>
    /// Interaction logic for TransactionView.xaml
    /// </summary>
    public partial class TransactionView : UserControl
    {
        public TransactionView()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            TransactionView_ViewModel tv = this.DataContext as TransactionView_ViewModel;
            string cat = Cat_List.SelectedValue.ToString();
            foreach (Transaction transaction in TransactionsGrid.SelectedItems)
            {

                transaction.Category = cat;
                transaction.company.category = cat;
                DatabaseAccess.update_category(transaction);
            }
            tv.LoadTransactions();

        }
    }
}
