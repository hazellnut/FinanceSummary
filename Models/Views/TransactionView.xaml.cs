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

                Categorise(transaction, cat);
            }
            tv.LoadTransactions();

        }

        private void Categorise(Transaction transaction,string category)
        {
            transaction.Category = category;
            transaction.company.category = category;
            DatabaseAccess.update_category(transaction);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            TransactionView_ViewModel tv = this.DataContext as TransactionView_ViewModel;
            List<KeywordPair> keywords = DatabaseAccess.get_keywords();
            List<string> word_pool = new();
            List<int> updateids = new();
            foreach (Transaction tr in tv.Transactions)
            {
                if (tr.company.category  == "Uncategorised")
                {
                    //List<string> split_words = tr.company.companystring.Split(' ').ToList();
                    //foreach (string word in split_words)
                    //{
                    KeywordPair x = keywords.Where(x => (tr.company.companystring.ToUpper().Contains(x.keyword.ToUpper()))).FirstOrDefault();
                    if (x != null)
                    {
                        Categorise(tr, x.category);
                        updateids.Add(tr.id);
                    }

                    Transaction same_company = tv.Transactions.Where(x => (x.company.companystring == tr.company.companystring) && (x.company.category != "Uncategorised")).FirstOrDefault();
                    if (same_company != null)
                    {
                        Categorise(tr, same_company.company.category);
                        updateids.Add(tr.id);
                    }

                }
            }

            tv.LoadTransactions();

            SetUpdated(tv.Transactions,updateids);
            tv.Transactions = tv.Transactions;
        }

        private void SetUpdated(List<Transaction>trs ,List<int> ids)
        {
            foreach (int id in ids)
            {
                Transaction tr = trs.Where(x => x.id == id).First();
                tr.JustUpdated = true;
                
            }
        }

        private void find_common_string_occurances(List<string> strings)
        {
            throw new NotImplementedException();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            TransactionView_ViewModel tv = this.DataContext as TransactionView_ViewModel;
            foreach (Transaction tr in tv.Transactions)
            {
                if (tr.JustUpdated)
                {
                    tr.JustUpdated = false;
                }
            }
            tv.Transactions = tv.Transactions;
        }
    }
}
